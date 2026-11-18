namespace ExerciciosService.Entidades
{
    public class Exercicio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public string Equipamento { get; set; } = string.Empty;
        public string NivelDificuldade { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
