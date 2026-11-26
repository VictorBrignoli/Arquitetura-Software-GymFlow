namespace TreinosService.Entidades
{
    public class Treino
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }              // ID do serviço de Alunos (sem FK real)
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;

        public List<TreinoItem> Itens { get; set; } = new();
    }
}