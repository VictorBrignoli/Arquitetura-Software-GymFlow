namespace TreinosService.Infra
{
    public static class GeradorDeServicos
    {
        public static ServiceProvider ServiceProvider;

        public static DataContext CarregarContexto()
        {
            return ServiceProvider.GetService<DataContext>();
        }

        // HttpClient já configurado (URL + timeout) no Program.cs: "alunos", "exercicios", "avaliacoes", "ia".
        public static HttpClient CarregarHttpClient(string nome)
        {
            return ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(nome);
        }
    }

    // Lançada quando um serviço de que dependemos não responde (vira HTTP 503).
    public class ServicoIndisponivelException : Exception
    {
        public ServicoIndisponivelException(string mensagem) : base(mensagem)
        {
        }
    }
}