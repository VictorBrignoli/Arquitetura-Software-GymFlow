using Microsoft.EntityFrameworkCore;
using TreinosService.Entidades;

namespace TreinosService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Treino> Treinos { get; set; }
        public DbSet<TreinoItem> TreinoItens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Treino>().HasKey(p => p.Id);
            modelBuilder.Entity<Treino>().Property(p => p.Nome).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Treino>().HasIndex(p => p.AlunoId);
            modelBuilder.Entity<Treino>()
                .HasMany(t => t.Itens)
                .WithOne(i => i.Treino)
                .HasForeignKey(i => i.TreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TreinoItem>().HasKey(p => p.Id);

            // Dados de demonstração (o banco é recriado a cada deploy no Render gratuito).
            // IDs de aluno e exercício batem com os seeds de Alunos e Exercícios.
            modelBuilder.Entity<Treino>().HasData(
                new { Id = 1, AlunoId = 2, Nome = "Treino A - Peito e Tríceps", DataCriacao = new DateTime(2026, 11, 1), Ativo = true },
                new { Id = 2, AlunoId = 1, Nome = "Treino Full Body", DataCriacao = new DateTime(2026, 11, 1), Ativo = true }
            );
            modelBuilder.Entity<TreinoItem>().HasData(
                new { Id = 1, TreinoId = 1, ExercicioId = 1, Series = 4, Repeticoes = 10, DescansoSegundos = 90, Ordem = 1 },
                new { Id = 2, TreinoId = 1, ExercicioId = 2, Series = 3, Repeticoes = 12, DescansoSegundos = 75, Ordem = 2 },
                new { Id = 3, TreinoId = 1, ExercicioId = 29, Series = 3, Repeticoes = 12, DescansoSegundos = 60, Ordem = 3 },
                new { Id = 4, TreinoId = 2, ExercicioId = 12, Series = 3, Repeticoes = 15, DescansoSegundos = 60, Ordem = 1 },
                new { Id = 5, TreinoId = 2, ExercicioId = 6, Series = 3, Repeticoes = 12, DescansoSegundos = 60, Ordem = 2 },
                new { Id = 6, TreinoId = 2, ExercicioId = 33, Series = 3, Repeticoes = 20, DescansoSegundos = 45, Ordem = 3 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}