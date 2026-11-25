namespace AvaliacoesService.DTO
{
    public class AvaliacaoDTO
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public DateTime Data { get; set; }
        public double Peso { get; set; }
        public double? PercentualGordura { get; set; }
        public double? Cintura { get; set; }
        public double? Quadril { get; set; }
        public double? Braco { get; set; }
        public string? Observacoes { get; set; }
        public bool Sincronizada { get; set; }
    }
}
