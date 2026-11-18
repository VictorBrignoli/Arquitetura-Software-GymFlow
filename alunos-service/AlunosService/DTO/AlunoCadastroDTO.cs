namespace AlunosService.DTO
{
    // Usado no POST (cadastro) e no PUT (edição) de alunos.
    public class AlunoCadastroDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string Objetivo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public double PesoAtual { get; set; }
        public int AlturaCm { get; set; }
        public double? MetaPeso { get; set; }

        // Opcional: no cadastro o aluno nasce ativo; no PUT permite ativar ou inativar.
        public bool? Ativo { get; set; }
    }
}
