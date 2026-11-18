using AlunosService.DTO;
using AlunosService.Entidades;
using AlunosService.Infra;

namespace AlunosService.Servicos
{
    public class ServAlunos
    {
        private static readonly string[] NiveisValidos = { "Iniciante", "Intermediario", "Avancado" };

        private DataContext _dataContext;

        public ServAlunos()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }

        public List<AlunoDTO> Listar(bool? ativo)
        {
            var consulta = _dataContext.Alunos.AsQueryable();

            if (ativo.HasValue)
                consulta = consulta.Where(a => a.Ativo == ativo.Value);

            return consulta
                .OrderBy(a => a.Nome)
                .AsEnumerable()
                .Select(ParaDTO)
                .ToList();
        }

        public AlunoDTO Buscar(int id)
        {
            return ParaDTO(CarregarAluno(id));
        }

        public AlunoDTO Cadastrar(AlunoCadastroDTO dto)
        {
            Validar(dto, idAtual: null);

            var aluno = new Aluno { Ativo = dto.Ativo ?? true };
            CopiarDados(dto, aluno);

            _dataContext.Alunos.Add(aluno);
            _dataContext.SaveChanges();

            return ParaDTO(aluno);
        }

        public AlunoDTO Atualizar(int id, AlunoCadastroDTO dto)
        {
            var aluno = CarregarAluno(id);
            Validar(dto, idAtual: id);

            CopiarDados(dto, aluno);
            if (dto.Ativo.HasValue)
                aluno.Ativo = dto.Ativo.Value;

            _dataContext.SaveChanges();

            return ParaDTO(aluno);
        }

        // Integração 3: o serviço de Avaliações envia o peso medido na avaliação mais recente.
        public AlunoDTO AtualizarMedidas(int id, MedidasDTO dto)
        {
            var aluno = CarregarAluno(id);

            if (dto.PesoAtual <= 0)
                throw new ArgumentException("O peso deve ser maior que zero.");
            if (dto.AlturaCm.HasValue && dto.AlturaCm.Value <= 0)
                throw new ArgumentException("A altura deve ser maior que zero.");

            aluno.PesoAtual = dto.PesoAtual;
            if (dto.AlturaCm.HasValue)
                aluno.AlturaCm = dto.AlturaCm.Value;

            _dataContext.SaveChanges();

            return ParaDTO(aluno);
        }

        private Aluno CarregarAluno(int id)
        {
            var aluno = _dataContext.Alunos.Find(id);

            if (aluno == null)
                throw new KeyNotFoundException($"Aluno {id} não encontrado.");

            return aluno;
        }

        private void Validar(AlunoCadastroDTO dto, int? idAtual)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
                throw new ArgumentException("Informe um e-mail válido.");
            if (dto.DataNascimento == default || dto.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Informe uma data de nascimento válida.");
            if (string.IsNullOrWhiteSpace(dto.Objetivo))
                throw new ArgumentException("O objetivo é obrigatório.");
            if (NormalizarNivel(dto.Nivel) == null)
                throw new ArgumentException($"Nível inválido. Use: {string.Join(", ", NiveisValidos)}.");
            if (dto.PesoAtual <= 0)
                throw new ArgumentException("O peso deve ser maior que zero.");
            if (dto.AlturaCm <= 0)
                throw new ArgumentException("A altura deve ser maior que zero.");
            if (dto.MetaPeso.HasValue && dto.MetaPeso.Value <= 0)
                throw new ArgumentException("A meta de peso deve ser maior que zero.");

            var email = dto.Email.Trim().ToLower();
            var emailEmUso = _dataContext.Alunos.Any(a => a.Email == email && a.Id != idAtual);
            if (emailEmUso)
                throw new ArgumentException("Já existe um aluno com este e-mail.");
        }

        private static void CopiarDados(AlunoCadastroDTO dto, Aluno aluno)
        {
            aluno.Nome = dto.Nome.Trim();
            aluno.Email = dto.Email.Trim().ToLower();
            aluno.DataNascimento = dto.DataNascimento;
            aluno.Objetivo = dto.Objetivo.Trim();
            aluno.Nivel = NormalizarNivel(dto.Nivel)!;
            aluno.PesoAtual = dto.PesoAtual;
            aluno.AlturaCm = dto.AlturaCm;
            aluno.MetaPeso = dto.MetaPeso;
        }

        private static string? NormalizarNivel(string? nivel)
        {
            return NiveisValidos.FirstOrDefault(n => string.Equals(n, nivel?.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static AlunoDTO ParaDTO(Aluno aluno)
        {
            return new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                DataNascimento = aluno.DataNascimento,
                Objetivo = aluno.Objetivo,
                Nivel = aluno.Nivel,
                PesoAtual = aluno.PesoAtual,
                AlturaCm = aluno.AlturaCm,
                MetaPeso = aluno.MetaPeso,
                Ativo = aluno.Ativo
            };
        }
    }
}
