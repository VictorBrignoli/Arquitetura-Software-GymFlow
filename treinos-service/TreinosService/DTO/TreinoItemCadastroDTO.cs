namespace TreinosService.DTO
{
    public class TreinoItemCadastroDTO
    {
        public int ExercicioId { get; set; }
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public int DescansoSegundos { get; set; } = 60;
        public int Ordem { get; set; }   // 0 = vai para o fim da lista
    }
}
