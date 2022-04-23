using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DeactivatedToken> DeactivatedTokens { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<User>()
                .HasMany(x => x.Favorites)
                .WithMany(x => x.Users);

            modelBuilder.Entity<DeactivatedToken>().HasKey(x => x.Id);

            modelBuilder.Entity<Movie>().HasKey(x => x.Id);
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Genres)
                .WithMany(x => x.Movies);

            modelBuilder.Entity<Genre>().HasKey(x => x.Id);
        }
    }
}
