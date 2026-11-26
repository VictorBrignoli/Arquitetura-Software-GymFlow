namespace TreinosService.DTO
{
    public class ExercicioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public string NivelDificuldade { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
