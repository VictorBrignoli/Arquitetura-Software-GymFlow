using System.Text;
using System.Text.Json;
using TreinosService.DTO;
using TreinosService.Entidades;
using TreinosService.Infra;

namespace TreinosService.Servicos
{
    public partial class ServTreino
    {
        private static readonly JsonSerializerOptions OpcoesJson = new(JsonSerializerDefaults.Web);

        public class PlanoIa
        {
            public string? Nome { get; set; }
            public List<ItemIa> Itens { get; set; } = new();
        }

        public class ItemIa
        {
            public int ExercicioId { get; set; }
            public int Series { get; set; }
            public int Repeticoes { get; set; }
            public int DescansoSegundos { get; set; }
        }

        // POST /api/treinos/gerar-ia
        public async Task<TreinoGeradoDTO> GerarComIa(GerarTreinoIaDTO dto)
        {
            // 1) aluno (obrigatório)
            var aluno = await _clientes.BuscarAluno(dto.AlunoId)
                ?? throw new KeyNotFoundException($"Aluno {dto.AlunoId} não encontrado.");
            if (!aluno.Ativo)
                throw new ArgumentException($"O aluno {aluno.Id} está inativo.");

            // 2) catálogo (obrigatório: sem ele a IA inventaria exercícios)
            var catalogo = await _clientes.ListarExerciciosAtivos();
            if (catalogo.Count == 0)
                throw new ArgumentException("O catálogo de exercícios está vazio.");

            // 3) última avaliação (opcional; o serviço já devolve a mais recente primeiro)
            AvaliacaoDTO? ultima = null;
            try { ultima = (await _clientes.ListarAvaliacoes(aluno.Id)).FirstOrDefault(); }
            catch (ServicoIndisponivelException) { }

            // 4) IA -> validação -> fallback
            var texto = await new ClienteIA().Gerar(MontarPrompt(aluno, ultima, catalogo));
            var plano = InterpretarResposta(texto, catalogo);
            var geradoPorIa = plano != null;
            plano ??= TreinoModelo(aluno, catalogo);

            // 5) salva como um treino comum (editável depois)
            var treino = new Treino
            {
                AlunoId = aluno.Id,
                Nome = plano.Nome!,
                Itens = plano.Itens.Select((i, indice) => new TreinoItem
                {
                    ExercicioId = i.ExercicioId,
                    Series = i.Series,
                    Repeticoes = i.Repeticoes,
                    DescansoSegundos = i.DescansoSegundos,
                    Ordem = indice + 1
                }).ToList()
            };
            _dataContext.Treinos.Add(treino);
            _dataContext.SaveChanges();

            return new TreinoGeradoDTO
            {
                GeradoPorIa = geradoPorIa,
                Treino = MontarDTO(treino, aluno, catalogo.ToDictionary(e => e.Id))
            };
        }

        // Não envia nome nem e-mail do aluno para a IA: só o necessário.
        private static string MontarPrompt(AlunoDTO aluno, AvaliacaoDTO? ultima, List<ExercicioDTO> catalogo)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Você é um personal trainer. Monte UM treino para o aluno abaixo.");
            sb.AppendLine($"Objetivo: {aluno.Objetivo}. Nível: {aluno.Nivel}. Peso atual: {aluno.PesoAtual} kg." +
                          (aluno.MetaPeso.HasValue ? $" Meta de peso: {aluno.MetaPeso} kg." : ""));
            if (ultima != null)
                sb.AppendLine($"Última avaliação ({ultima.Data:dd/MM/yyyy}): peso {ultima.Peso} kg, " +
                              $"gordura {ultima.PercentualGordura}%.");
            sb.AppendLine();
            sb.AppendLine("Use SOMENTE exercícios desta lista (id | nome | grupo | nível):");
            foreach (var e in catalogo)
                sb.AppendLine($"{e.Id} | {e.Nome} | {e.GrupoMuscular} | {e.NivelDificuldade}");
            sb.AppendLine();
            sb.AppendLine("Regras: entre 5 e 8 exercícios; series de 1 a 10; repeticoes de 1 a 30; " +
                          "descansoSegundos de 30 a 180; respeite o nível do aluno; dê um nome curto ao treino.");
            sb.AppendLine("Responda APENAS com JSON neste formato, sem nenhum texto extra:");
            sb.AppendLine("{\"nome\": \"string\", \"itens\": [{\"exercicioId\": 0, \"series\": 0, " +
                          "\"repeticoes\": 0, \"descansoSegundos\": 0}]}");
            return sb.ToString();
        }

        // Valida tudo o que veio da IA; devolve null se não sobrar nada aproveitável.
        private static PlanoIa? InterpretarResposta(string? texto, List<ExercicioDTO> catalogo)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            var inicio = texto.IndexOf('{');          // remove eventual ```json ... ```
            var fim = texto.LastIndexOf('}');
            if (inicio < 0 || fim <= inicio)
                return null;
            texto = texto[inicio..(fim + 1)];

            PlanoIa? plano;
            try { plano = JsonSerializer.Deserialize<PlanoIa>(texto, OpcoesJson); }
            catch (JsonException) { return null; }
            if (plano == null)
                return null;

            var idsValidos = catalogo.Select(e => e.Id).ToHashSet();
            plano.Itens = plano.Itens
                .Where(i => idsValidos.Contains(i.ExercicioId)
                         && i.Series is >= 1 and <= 10
                         && i.Repeticoes is >= 1 and <= 30
                         && i.DescansoSegundos is >= 0 and <= 300)
                .DistinctBy(i => i.ExercicioId)
                .Take(10)
                .ToList();

            if (plano.Itens.Count == 0)
                return null;

            plano.Nome = string.IsNullOrWhiteSpace(plano.Nome) ? "Treino gerado por IA" : plano.Nome.Trim();
            if (plano.Nome.Length > 100)
                plano.Nome = plano.Nome[..100];

            return plano;
        }

        // Fallback: 1 exercício por grupo muscular (sem cardio), carga pelo nível do aluno.
        private static PlanoIa TreinoModelo(AlunoDTO aluno, List<ExercicioDTO> catalogo)
        {
            var (series, repeticoes, descanso) = aluno.Nivel switch
            {
                "Avancado" => (4, 8, 90),
                "Intermediario" => (4, 10, 75),
                _ => (3, 12, 60)
            };

            var escolhidos = catalogo
                .Where(e => e.GrupoMuscular != "Cardio")
                .GroupBy(e => e.GrupoMuscular)
                .Select(g => g.OrderBy(e => e.NivelDificuldade == "Iniciante" ? 0 : 1).First())
                .Take(6)
                .ToList();

            if (escolhidos.Count == 0)
                escolhidos = catalogo.Take(3).ToList();

            return new PlanoIa
            {
                Nome = "Treino modelo - corpo inteiro",
                Itens = escolhidos.Select(e => new ItemIa
                {
                    ExercicioId = e.Id,
                    Series = series,
                    Repeticoes = repeticoes,
                    DescansoSegundos = descanso
                }).ToList()
            };
        }
    }
}