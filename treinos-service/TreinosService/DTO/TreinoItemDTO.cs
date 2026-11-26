namespace TreinosService.DTO
{
    public class TreinoItemDTO
    {
        public int Id { get; set; }
        public int ExercicioId { get; set; }
        public string ExercicioNome { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public int DescansoSegundos { get; set; }
        public int Ordem { get; set; }
    }
}
