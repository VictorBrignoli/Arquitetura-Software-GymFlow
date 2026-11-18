namespace AlunosService.DTO
{
    public class AlunoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string Objetivo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public double PesoAtual { get; set; }
        public int AlturaCm { get; set; }
        public double? MetaPeso { get; set; }
        public bool Ativo { get; set; }
    }
}
