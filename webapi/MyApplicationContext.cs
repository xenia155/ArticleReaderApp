using Microsoft.EntityFrameworkCore;

namespace webapi
{
    public class MyApplicationContext : DbContext
    {
       // public DbSet<Article> Articles { get; set; }
        public DbSet<Entity> Entities { get; set; }

        private const string Connection = "Host=localhost;Port=54320;Database=webarticlesdb;Username=admin;Password=admin";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Connection);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

    }
}
