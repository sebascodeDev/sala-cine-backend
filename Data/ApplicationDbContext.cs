using Microsoft.EntityFrameworkCore;

namespace SalaCine.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Model.Entities.Pelicula> Peliculas { get; set; }
    }
}