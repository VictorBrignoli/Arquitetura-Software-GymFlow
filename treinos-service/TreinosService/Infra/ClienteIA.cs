using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TreinosService.Infra
{
    // Chama a API do Google Gemini. Retorna o texto (JSON) gerado ou null se algo falhar.
    public class ClienteIA
    {
        private HttpClient _http;

        public ClienteIA()
        {
            _http = GeradorDeServicos.CarregarHttpClient("ia");
        }

        public async Task<string?> Gerar(string prompt)
        {
            var chave = Environment.GetEnvironmentVariable("IA_API_KEY");
            if (string.IsNullOrWhiteSpace(chave))
            {
                Console.WriteLine("[IA] IA_API_KEY não configurada; usando treino-modelo.");
                return null;
            }

            var modelo = Environment.GetEnvironmentVariable("IA_MODELO") ?? "gemini-3.5-flash-lite";
            var corpo = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } },
                generationConfig = new { responseMimeType = "application/json", temperature = 0.4 }
            };

            using var requisicao = new HttpRequestMessage(HttpMethod.Post, $"v1beta/models/{modelo}:generateContent")
            {
                Content = JsonContent.Create(corpo)
            };
            requisicao.Headers.Add("x-goog-api-key", chave);

            try
            {
                var resposta = await _http.SendAsync(requisicao);
                var conteudo = await resposta.Content.ReadAsStringAsync();

                if (!resposta.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[IA] Erro {(int)resposta.StatusCode}: {conteudo}");
                    return null;
                }

                using var doc = JsonDocument.Parse(conteudo);
                var partes = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts");

                // Junta só os trechos de texto (ignora partes de "raciocínio" do modelo)
                var texto = new StringBuilder();
                foreach (var parte in partes.EnumerateArray())
                {
                    var ehPensamento = parte.TryGetProperty("thought", out var pensamento) && pensamento.GetBoolean();
                    if (!ehPensamento && parte.TryGetProperty("text", out var trecho))
                        texto.Append(trecho.GetString());
                }

                return texto.Length > 0 ? texto.ToString() : null;
            }
            catch (Exception e)   // qualquer falha da IA vira fallback; nunca derruba o serviço
            {
                Console.WriteLine($"[IA] Falha: {e.Message}");
                return null;
            }
        }
    }
}