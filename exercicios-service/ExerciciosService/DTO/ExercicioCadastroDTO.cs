namespace ExerciciosService.DTO
{
    // Usado no POST (cadastro) e no PUT (edição) de exercícios.
    public class ExercicioCadastroDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public string Equipamento { get; set; } = string.Empty;
        public string NivelDificuldade { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        // Opcional: no cadastro o exercício nasce ativo; no PUT permite ativar ou inativar.
        public bool? Ativo { get; set; }
    }
}
