using ExerciciosService.DTO;
using ExerciciosService.Entidades;
using ExerciciosService.Infra;

namespace ExerciciosService.Servicos
{
    public class ServExercicios
    {
        private static readonly string[] NiveisValidos = { "Iniciante", "Intermediario", "Avancado" };

        private DataContext _dataContext;

        public ServExercicios()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }

        public List<ExercicioDTO> Listar(string? grupo, bool? ativo)
        {
            var consulta = _dataContext.Exercicios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(grupo))
            {
                var grupoNormalizado = grupo.Trim().ToLower();
                consulta = consulta.Where(e => e.GrupoMuscular.ToLower() == grupoNormalizado);
            }

            if (ativo.HasValue)
                consulta = consulta.Where(e => e.Ativo == ativo.Value);

            return consulta
                .OrderBy(e => e.GrupoMuscular)
                .ThenBy(e => e.Nome)
                .AsEnumerable()
                .Select(ParaDTO)
                .ToList();
        }

        public ExercicioDTO Buscar(int id)
        {
            return ParaDTO(CarregarExercicio(id));
        }

        public ExercicioDTO Cadastrar(ExercicioCadastroDTO dto)
        {
            Validar(dto);

            var exercicio = new Exercicio { Ativo = dto.Ativo ?? true };
            CopiarDados(dto, exercicio);

            _dataContext.Exercicios.Add(exercicio);
            _dataContext.SaveChanges();

            return ParaDTO(exercicio);
        }

        public ExercicioDTO Atualizar(int id, ExercicioCadastroDTO dto)
        {
            var exercicio = CarregarExercicio(id);
            Validar(dto);

            CopiarDados(dto, exercicio);
            if (dto.Ativo.HasValue)
                exercicio.Ativo = dto.Ativo.Value;

            _dataContext.SaveChanges();

            return ParaDTO(exercicio);
        }

        private Exercicio CarregarExercicio(int id)
        {
            var exercicio = _dataContext.Exercicios.Find(id);

            if (exercicio == null)
                throw new KeyNotFoundException($"Exercício {id} não encontrado.");

            return exercicio;
        }

        private static void Validar(ExercicioCadastroDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.GrupoMuscular))
                throw new ArgumentException("O grupo muscular é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Equipamento))
                throw new ArgumentException("O equipamento é obrigatório.");
            if (NormalizarNivel(dto.NivelDificuldade) == null)
                throw new ArgumentException($"Nível de dificuldade inválido. Use: {string.Join(", ", NiveisValidos)}.");
        }

        private static void CopiarDados(ExercicioCadastroDTO dto, Exercicio exercicio)
        {
            exercicio.Nome = dto.Nome.Trim();
            exercicio.GrupoMuscular = dto.GrupoMuscular.Trim();
            exercicio.Equipamento = dto.Equipamento.Trim();
            exercicio.NivelDificuldade = NormalizarNivel(dto.NivelDificuldade)!;
            exercicio.Descricao = dto.Descricao?.Trim() ?? string.Empty;
        }

        private static string? NormalizarNivel(string? nivel)
        {
            return NiveisValidos.FirstOrDefault(n => string.Equals(n, nivel?.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static ExercicioDTO ParaDTO(Exercicio exercicio)
        {
            return new ExercicioDTO
            {
                Id = exercicio.Id,
                Nome = exercicio.Nome,
                GrupoMuscular = exercicio.GrupoMuscular,
                Equipamento = exercicio.Equipamento,
                NivelDificuldade = exercicio.NivelDificuldade,
                Descricao = exercicio.Descricao,
                Ativo = exercicio.Ativo
            };
        }
    }
}
