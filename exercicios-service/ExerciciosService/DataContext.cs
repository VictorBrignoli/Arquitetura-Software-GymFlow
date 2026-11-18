using ExerciciosService.Entidades;
using ExerciciosService.Infra;
using Microsoft.EntityFrameworkCore;

namespace ExerciciosService
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Exercicio> Exercicios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercicio>().HasKey(p => p.Id);
            modelBuilder.Entity<Exercicio>().HasIndex(p => p.GrupoMuscular);

            // Catálogo inicial (o banco é recriado a cada deploy no Render gratuito).
            modelBuilder.Entity<Exercicio>().HasData(DadosIniciais.Exercicios);

            base.OnModelCreating(modelBuilder);
        }
    }
}
