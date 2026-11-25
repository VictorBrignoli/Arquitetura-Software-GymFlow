using AvaliacoesService.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AvaliacoesService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Avaliacao> Avaliacoes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Avaliacao>().HasKey(p => p.Id);

            // Dados de demonstração (o banco é recriado a cada deploy no Render gratuito).
            modelBuilder.Entity<Avaliacao>().HasData(
                new Avaliacao { Id = 1, AlunoId = 1, Data = new DateTime(2026, 9, 1), Peso = 75.0, PercentualGordura = 31.0, Cintura = 84, Quadril = 104, Braco = 29, Observacoes = "Avaliação inicial", Sincronizada = true },
                new Avaliacao { Id = 2, AlunoId = 1, Data = new DateTime(2026, 10, 15), Peso = 72.5, PercentualGordura = 29.5, Cintura = 82, Quadril = 102, Braco = 29.5, Observacoes = "Boa evolução", Sincronizada = true },
                new Avaliacao { Id = 3, AlunoId = 2, Data = new DateTime(2026, 10, 5), Peso = 78.0, PercentualGordura = 16.0, Cintura = 82, Quadril = 98, Braco = 35, Observacoes = "Avaliação inicial", Sincronizada = true }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
