namespace AvaliacoesService.DTO
{
    // Corpo do POST /api/avaliacoes.
    public class AvaliacaoCadastroDTO
    {
        public int AlunoId { get; set; }

        // Opcional: se não for informada, usa a data/hora atual.
        public DateTime? Data { get; set; }

        public double Peso { get; set; }
        public double? PercentualGordura { get; set; }
        public double? Cintura { get; set; }
        public double? Quadril { get; set; }
        public double? Braco { get; set; }
        public string? Observacoes { get; set; }
    }
}
