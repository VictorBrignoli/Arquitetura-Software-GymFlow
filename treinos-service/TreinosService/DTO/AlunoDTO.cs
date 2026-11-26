namespace TreinosService.DTO
{
    public class AlunoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public double PesoAtual { get; set; }
        public double? MetaPeso { get; set; }
        public bool Ativo { get; set; }
    }
}