using Microsoft.EntityFrameworkCore;

namespace SalaCine.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Define DbSets for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}