using Microsoft.EntityFrameworkCore;

namespace Covercy_HomeAssignment
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ApiToken> ApiTokens => Set<ApiToken>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bin\\Debug\\net8.0\\Covercy_HA_Vatnik.db");
        }
    }
}
