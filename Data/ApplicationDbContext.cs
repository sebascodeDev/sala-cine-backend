using Microsoft.EntityFrameworkCore;
using SalaCine.Api.Model.Entities;

namespace SalaCine.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<PeliculaSala> PeliculasSalas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones
            modelBuilder.Entity<PeliculaSala>()
                .HasOne(ps => ps.Pelicula)
                .WithMany()
                .HasForeignKey(ps => ps.PeliculaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PeliculaSala>()
                .HasOne(ps => ps.Sala)
                .WithMany(s => s.PeliculasSalas)
                .HasForeignKey(ps => ps.SalaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}