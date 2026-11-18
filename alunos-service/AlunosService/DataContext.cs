using AlunosService.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AlunosService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Aluno> Alunos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().HasKey(p => p.Id);
            modelBuilder.Entity<Aluno>().HasIndex(p => p.Email).IsUnique();

            // Dados de demonstração (o banco é recriado a cada deploy no Render gratuito).
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno { Id = 1, Nome = "Ana Souza", Email = "ana.souza@email.com", DataNascimento = new DateOnly(1998, 3, 14), Objetivo = "Emagrecimento", Nivel = "Iniciante", PesoAtual = 72.5, AlturaCm = 165, MetaPeso = 65, Ativo = true },
                new Aluno { Id = 2, Nome = "Bruno Lima", Email = "bruno.lima@email.com", DataNascimento = new DateOnly(1995, 7, 2), Objetivo = "Hipertrofia", Nivel = "Intermediario", PesoAtual = 78, AlturaCm = 180, MetaPeso = 84, Ativo = true },
                new Aluno { Id = 3, Nome = "Carla Mendes", Email = "carla.mendes@email.com", DataNascimento = new DateOnly(1990, 11, 23), Objetivo = "Condicionamento", Nivel = "Avancado", PesoAtual = 61, AlturaCm = 170, MetaPeso = null, Ativo = true },
                new Aluno { Id = 4, Nome = "Diego Rocha", Email = "diego.rocha@email.com", DataNascimento = new DateOnly(2001, 1, 9), Objetivo = "Hipertrofia", Nivel = "Iniciante", PesoAtual = 70, AlturaCm = 175, MetaPeso = 76, Ativo = false }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
