namespace AvaliacoesService.DTO
{
    // Resposta de GET /api/alunos/{id} do serviço de Alunos (só os campos que usamos).
    public class AlunoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double PesoAtual { get; set; }
        public bool Ativo { get; set; }
    }
}
