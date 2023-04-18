using Microsoft.EntityFrameworkCore;

namespace webapi
{
    public class ApplicationContext: DbContext
    {
        public DbSet<WebArticleContent> Articles { get; set; } = null!;
        public DbSet<WebArticleKeywords> ArticlesKeywords { get; set; } = null!;

        private const string ConnectionString = "Host=localhost;Port=54320;Database=webarticlestestdb;Username=admin;Password=admin";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior",true);
        }
        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebArticleKeywords>()
                .HasIndex(x => x.articleKeyword);
            modelBuilder.Entity<WebArticleContent>()
                .HasIndex(x => x.articleUrl);
        }*/

    }
}
