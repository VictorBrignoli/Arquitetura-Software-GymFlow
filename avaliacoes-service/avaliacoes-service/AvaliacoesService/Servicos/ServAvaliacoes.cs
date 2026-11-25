using System.Text.Json;
using AvaliacoesService.DTO;
using AvaliacoesService.Entidades;
using AvaliacoesService.Infra;

namespace AvaliacoesService.Servicos
{
    public class ServAvaliacoes
    {
        private DataContext _dataContext;
        private HttpClient _alunosHttp;

        public ServAvaliacoes()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
            _alunosHttp = GeradorDeServicos.CarregarHttpClientAlunos();
        }

        // Integração 3 (Avaliações -> Alunos): valida o aluno, salva a avaliação e atualiza o peso no cadastro.
        public async Task<AvaliacaoDTO> Registrar(AvaliacaoCadastroDTO dto)
        {
            Validar(dto);

            // 1) valida o aluno no serviço de Alunos (404 / inativo / fora do ar interrompem aqui, sem gravar nada)
            var aluno = await BuscarAluno(dto.AlunoId);
            if (!aluno.Ativo)
                throw new ArgumentException($"O aluno {aluno.Id} está inativo.");

            // 2) salva a avaliação ainda como não sincronizada
            var avaliacao = new Avaliacao
            {
                AlunoId = aluno.Id,
                Data = dto.Data ?? DateTime.Now,
                Peso = dto.Peso,
                PercentualGordura = dto.PercentualGordura,
                Cintura = dto.Cintura,
                Quadril = dto.Quadril,
                Braco = dto.Braco,
                Observacoes = dto.Observacoes?.Trim(),
                Sincronizada = false
            };

            _dataContext.Avaliacoes.Add(avaliacao);
            _dataContext.SaveChanges();

            // 3) propaga o peso para o cadastro do aluno; se falhar, fica Sincronizada = false
            avaliacao.Sincronizada = await AtualizarPesoNoAluno(avaliacao.AlunoId, avaliacao.Peso);
            if (avaliacao.Sincronizada)
                _dataContext.SaveChanges();

            return ParaDTO(avaliacao);
        }

        public AvaliacaoDTO Buscar(int id)
        {
            var avaliacao = _dataContext.Avaliacoes.Find(id);

            if (avaliacao == null)
                throw new KeyNotFoundException($"Avaliação {id} não encontrada.");

            return ParaDTO(avaliacao);
        }

        public List<AvaliacaoDTO> ListarPorAluno(int alunoId)
        {
            return _dataContext.Avaliacoes
                .Where(a => a.AlunoId == alunoId)
                .OrderByDescending(a => a.Data)
                .ThenByDescending(a => a.Id)
                .AsEnumerable()
                .Select(ParaDTO)
                .ToList();
        }

        // Tenta de novo enviar ao serviço de Alunos o peso das avaliações com Sincronizada = false.
        // Vai da mais antiga para a mais nova, então o peso mais recente é o último a ser gravado.
        public async Task<SincronizacaoDTO> SincronizarPendentes()
        {
            var pendentes = _dataContext.Avaliacoes
                .Where(a => !a.Sincronizada)
                .OrderBy(a => a.Data)
                .ThenBy(a => a.Id)
                .ToList();

            var sincronizadas = 0;

            foreach (var avaliacao in pendentes)
            {
                if (await AtualizarPesoNoAluno(avaliacao.AlunoId, avaliacao.Peso))
                {
                    avaliacao.Sincronizada = true;
                    sincronizadas++;
                }
            }

            _dataContext.SaveChanges();

            return new SincronizacaoDTO { Pendentes = pendentes.Count, Sincronizadas = sincronizadas };
        }

        private async Task<AlunoDTO> BuscarAluno(int alunoId)
        {
            try
            {
                var resposta = await _alunosHttp.GetAsync($"api/alunos/{alunoId}");

                if (resposta.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new KeyNotFoundException($"Aluno {alunoId} não encontrado.");

                if (!resposta.IsSuccessStatusCode)
                    throw new ServicoIndisponivelException($"O serviço de Alunos respondeu com erro ({(int)resposta.StatusCode}).");

                var aluno = await resposta.Content.ReadFromJsonAsync<AlunoDTO>();

                return aluno ?? throw new ServicoIndisponivelException("O serviço de Alunos devolveu uma resposta vazia.");
            }
            catch (Exception e) when (e is HttpRequestException || e is TaskCanceledException || e is JsonException)
            {
                throw new ServicoIndisponivelException("O serviço de Alunos está indisponível. Tente novamente em instantes.");
            }
        }

        // Nunca lança exceção: devolve false se o serviço de Alunos não conseguiu atualizar.
        private async Task<bool> AtualizarPesoNoAluno(int alunoId, double peso)
        {
            try
            {
                var requisicao = new HttpRequestMessage(HttpMethod.Patch, $"api/alunos/{alunoId}/medidas")
                {
                    Content = JsonContent.Create(new { pesoAtual = peso })
                };

                var resposta = await _alunosHttp.SendAsync(requisicao);

                return resposta.IsSuccessStatusCode;
            }
            catch (Exception e) when (e is HttpRequestException || e is TaskCanceledException)
            {
                return false;
            }
        }

        private static void Validar(AvaliacaoCadastroDTO dto)
        {
            if (dto.AlunoId <= 0)
                throw new ArgumentException("Informe o aluno da avaliação.");
            if (dto.Peso <= 0 || dto.Peso > 500)
                throw new ArgumentException("O peso deve ser maior que zero e no máximo 500 kg.");
            if (dto.PercentualGordura.HasValue && (dto.PercentualGordura.Value < 0 || dto.PercentualGordura.Value > 100))
                throw new ArgumentException("O percentual de gordura deve estar entre 0 e 100.");
            if (dto.Cintura.HasValue && dto.Cintura.Value <= 0)
                throw new ArgumentException("A cintura deve ser maior que zero.");
            if (dto.Quadril.HasValue && dto.Quadril.Value <= 0)
                throw new ArgumentException("O quadril deve ser maior que zero.");
            if (dto.Braco.HasValue && dto.Braco.Value <= 0)
                throw new ArgumentException("O braço deve ser maior que zero.");
            if (dto.Data.HasValue && dto.Data.Value > DateTime.Now.AddDays(1))
                throw new ArgumentException("A data da avaliação não pode estar no futuro.");
        }

        private static AvaliacaoDTO ParaDTO(Avaliacao avaliacao)
        {
            return new AvaliacaoDTO
            {
                Id = avaliacao.Id,
                AlunoId = avaliacao.AlunoId,
                Data = avaliacao.Data,
                Peso = avaliacao.Peso,
                PercentualGordura = avaliacao.PercentualGordura,
                Cintura = avaliacao.Cintura,
                Quadril = avaliacao.Quadril,
                Braco = avaliacao.Braco,
                Observacoes = avaliacao.Observacoes,
                Sincronizada = avaliacao.Sincronizada
            };
        }
    }
}
