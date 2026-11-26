using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TreinosService.DTO;

namespace TreinosService.Infra
{
    // 404 -> null | erro HTTP, sem conexão, timeout ou JSON inválido -> ServicoIndisponivelException (503)
    public class ClienteServicos
    {
        private HttpClient _alunosHttp;
        private HttpClient _exerciciosHttp;
        private HttpClient _avaliacoesHttp;

        public ClienteServicos()
        {
            _alunosHttp = GeradorDeServicos.CarregarHttpClient("alunos");
            _exerciciosHttp = GeradorDeServicos.CarregarHttpClient("exercicios");
            _avaliacoesHttp = GeradorDeServicos.CarregarHttpClient("avaliacoes");
        }

        // Integração 1
        public Task<AlunoDTO?> BuscarAluno(int id)
            => Get<AlunoDTO>(_alunosHttp, $"api/alunos/{id}", "Alunos");

        // Integração 2
        public Task<ExercicioDTO?> BuscarExercicio(int id)
            => Get<ExercicioDTO>(_exerciciosHttp, $"api/exercicios/{id}", "Exercícios");

        // IA (extra)
        public async Task<List<ExercicioDTO>> ListarExerciciosAtivos()
            => await Get<List<ExercicioDTO>>(_exerciciosHttp, "api/exercicios?ativo=true", "Exercícios") ?? new();

        public async Task<List<AvaliacaoDTO>> ListarAvaliacoes(int alunoId)
            => await Get<List<AvaliacaoDTO>>(_avaliacoesHttp, $"api/avaliacoes/aluno/{alunoId}", "Avaliações") ?? new();

        private static async Task<T?> Get<T>(HttpClient http, string caminho, string servico) where T : class
        {
            try
            {
                var resposta = await http.GetAsync(caminho);

                if (resposta.StatusCode == HttpStatusCode.NotFound)
                    return null;

                if (!resposta.IsSuccessStatusCode)
                    throw new ServicoIndisponivelException(
                        $"O serviço de {servico} respondeu com erro ({(int)resposta.StatusCode}).");

                return await resposta.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception e) when (e is HttpRequestException || e is TaskCanceledException || e is JsonException)
            {
                throw new ServicoIndisponivelException(
                    $"O serviço de {servico} está indisponível. Tente novamente em instantes.");
            }
        }
    }
}