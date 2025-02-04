using Microsoft.EntityFrameworkCore;

namespace Movie_Catalog.Models
{
    public class MovieCatalogContext : DbContext
    {
        public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options) 
        : base(options) 
        {
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            
            modelBuilder.Entity<MovieActor>()
                .HasKey(pi => new { pi.MovieId, pi.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(pi => pi.Movie)
                .WithMany()
                .HasForeignKey(pi => pi.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(pi => pi.Actor)
                .WithMany()
                .HasForeignKey(pi => pi.ActorId);

            //Seed data
            //Тук ще добавим някои неща по default като жанровете,актьорите и режисьорите
        
        }
    }
}
