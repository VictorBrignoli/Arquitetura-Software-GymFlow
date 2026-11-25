namespace AvaliacoesService.Infra
{
    public static class GeradorDeServicos
    {
        public static ServiceProvider ServiceProvider;

        public static DataContext CarregarContexto()
        {
            return ServiceProvider.GetService<DataContext>();
        }

        // HttpClient já configurado com a URL (ALUNOS_URL) e o timeout do serviço de Alunos.
        public static HttpClient CarregarHttpClientAlunos()
        {
            return ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("alunos");
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
