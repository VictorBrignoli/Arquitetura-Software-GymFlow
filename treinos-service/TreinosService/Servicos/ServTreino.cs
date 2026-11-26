using Microsoft.EntityFrameworkCore;
using TreinosService.DTO;
using TreinosService.Entidades;
using TreinosService.Infra;

namespace TreinosService.Servicos
{
    public partial class ServTreino
    {
        private const string Indisponivel = "(indisponível)";

        private DataContext _dataContext;
        private ClienteServicos _clientes;

        public ServTreino()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
            _clientes = new ClienteServicos();
        }

        // Integração 1 (Treinos -> Alunos): valida o aluno antes de criar o treino.
        public async Task<TreinoDTO> Criar(TreinoCadastroDTO dto)
        {
            if (dto.AlunoId <= 0)
                throw new ArgumentException("Informe o aluno do treino.");
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome do treino é obrigatório.");
            if (dto.Nome.Trim().Length > 100)
                throw new ArgumentException("O nome do treino deve ter no máximo 100 caracteres.");

            var aluno = await _clientes.BuscarAluno(dto.AlunoId)
                ?? throw new KeyNotFoundException($"Aluno {dto.AlunoId} não encontrado.");
            if (!aluno.Ativo)
                throw new ArgumentException($"O aluno {aluno.Id} está inativo.");

            var treino = new Treino { AlunoId = aluno.Id, Nome = dto.Nome.Trim() };
            _dataContext.Treinos.Add(treino);
            _dataContext.SaveChanges();

            return MontarDTO(treino, aluno, new Dictionary<int, ExercicioDTO>());
        }

        // Integração 2 (Treinos -> Exercícios): valida o exercício antes de incluir o item.
        public async Task<TreinoItemDTO> AdicionarItem(int treinoId, TreinoItemCadastroDTO dto)
        {
            var treino = CarregarTreino(treinoId);

            if (!treino.Ativo)
                throw new ArgumentException("Não é possível alterar um treino inativo.");
            if (dto.ExercicioId <= 0)
                throw new ArgumentException("Informe o exercício.");
            if (dto.Series < 1 || dto.Series > 10)
                throw new ArgumentException("As séries devem estar entre 1 e 10.");
            if (dto.Repeticoes < 1 || dto.Repeticoes > 50)
                throw new ArgumentException("As repetições devem estar entre 1 e 50.");
            if (dto.DescansoSegundos < 0 || dto.DescansoSegundos > 600)
                throw new ArgumentException("O descanso deve estar entre 0 e 600 segundos.");

            var exercicio = await _clientes.BuscarExercicio(dto.ExercicioId)
                ?? throw new KeyNotFoundException($"Exercício {dto.ExercicioId} não encontrado.");
            if (!exercicio.Ativo)
                throw new ArgumentException($"O exercício {exercicio.Id} está inativo.");

            var ordem = dto.Ordem > 0
                ? dto.Ordem
                : (treino.Itens.Count == 0 ? 1 : treino.Itens.Max(i => i.Ordem) + 1);

            var item = new TreinoItem
            {
                ExercicioId = exercicio.Id,
                Series = dto.Series,
                Repeticoes = dto.Repeticoes,
                DescansoSegundos = dto.DescansoSegundos,
                Ordem = ordem
            };
            treino.Itens.Add(item);
            _dataContext.SaveChanges();

            return MontarItemDTO(item, exercicio);
        }

        // GET /api/treinos/{id}: enriquecido com o nome do aluno e o nome/grupo dos exercícios.
        public async Task<TreinoDTO> Buscar(int id)
        {
            var treino = CarregarTreino(id);

            var aluno = await BuscarAlunoSemFalhar(treino.AlunoId);
            var exercicios = await BuscarExerciciosSemFalhar(treino.Itens.Select(i => i.ExercicioId));

            return MontarDTO(treino, aluno, exercicios);
        }

        // GET /api/treinos/aluno/{alunoId}
        public async Task<List<TreinoDTO>> ListarPorAluno(int alunoId)
        {
            var treinos = _dataContext.Treinos
                .Include(t => t.Itens)
                .Where(t => t.AlunoId == alunoId)
                .OrderByDescending(t => t.DataCriacao)
                .ThenByDescending(t => t.Id)
                .ToList();

            if (treinos.Count == 0)
                return new List<TreinoDTO>();

            // 1 chamada para o aluno + 1 por exercício DISTINTO (não 1 por item)
            var aluno = await BuscarAlunoSemFalhar(alunoId);
            var exercicios = await BuscarExerciciosSemFalhar(
                treinos.SelectMany(t => t.Itens).Select(i => i.ExercicioId));

            return treinos.Select(t => MontarDTO(t, aluno, exercicios)).ToList();
        }

        // DELETE /api/treinos/{id}/itens/{itemId}
        public void RemoverItem(int treinoId, int itemId)
        {
            var item = _dataContext.TreinoItens.FirstOrDefault(i => i.Id == itemId && i.TreinoId == treinoId)
                ?? throw new KeyNotFoundException($"Item {itemId} não encontrado no treino {treinoId}.");

            _dataContext.TreinoItens.Remove(item);
            _dataContext.SaveChanges();
        }

        // ---------- auxiliares ----------

        private Treino CarregarTreino(int id)
        {
            return _dataContext.Treinos.Include(t => t.Itens).FirstOrDefault(t => t.Id == id)
                ?? throw new KeyNotFoundException($"Treino {id} não encontrado.");
        }

        // Na leitura, a queda de outro serviço não derruba a resposta.
        private async Task<AlunoDTO?> BuscarAlunoSemFalhar(int alunoId)
        {
            try { return await _clientes.BuscarAluno(alunoId); }
            catch (ServicoIndisponivelException) { return null; }
        }

        private async Task<Dictionary<int, ExercicioDTO>> BuscarExerciciosSemFalhar(IEnumerable<int> ids)
        {
            var exercicios = new Dictionary<int, ExercicioDTO>();

            foreach (var id in ids.Distinct())
            {
                try
                {
                    var exercicio = await _clientes.BuscarExercicio(id);
                    if (exercicio != null)
                        exercicios[id] = exercicio;
                }
                catch (ServicoIndisponivelException)
                {
                    // segue sem o nome deste exercício
                }
            }

            return exercicios;
        }

        private static TreinoDTO MontarDTO(Treino treino, AlunoDTO? aluno, Dictionary<int, ExercicioDTO> exercicios)
        {
            return new TreinoDTO
            {
                Id = treino.Id,
                AlunoId = treino.AlunoId,
                AlunoNome = aluno?.Nome ?? Indisponivel,
                AlunoObjetivo = aluno?.Objetivo,
                Nome = treino.Nome,
                DataCriacao = treino.DataCriacao,
                Ativo = treino.Ativo,
                Itens = treino.Itens
                    .OrderBy(i => i.Ordem)
                    .Select(i => MontarItemDTO(i, exercicios.GetValueOrDefault(i.ExercicioId)))
                    .ToList()
            };
        }

        private static TreinoItemDTO MontarItemDTO(TreinoItem item, ExercicioDTO? exercicio)
        {
            return new TreinoItemDTO
            {
                Id = item.Id,
                ExercicioId = item.ExercicioId,
                ExercicioNome = exercicio?.Nome ?? Indisponivel,
                GrupoMuscular = exercicio?.GrupoMuscular ?? Indisponivel,
                Series = item.Series,
                Repeticoes = item.Repeticoes,
                DescansoSegundos = item.DescansoSegundos,
                Ordem = item.Ordem
            };
        }
    }
}