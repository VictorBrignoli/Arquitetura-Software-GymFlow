namespace TreinosService.Entidades
{
    public class TreinoItem
    {
        public int Id { get; set; }

        public int TreinoId { get; set; }             // FK real: mesma base de dados
        public Treino? Treino { get; set; }

        public int ExercicioId { get; set; }          // ID do serviço de Exercícios (sem FK real)
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public int DescansoSegundos { get; set; }
        public int Ordem { get; set; }
    }
}