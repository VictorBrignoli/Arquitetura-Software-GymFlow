namespace TreinosService.DTO
{
    public class TreinoDTO
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string AlunoNome { get; set; } = string.Empty;
        public string? AlunoObjetivo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public List<TreinoItemDTO> Itens { get; set; } = new();
    }
}
