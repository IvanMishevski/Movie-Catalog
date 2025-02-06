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
            modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action", CreatedAt = DateTime.UtcNow },
            new Genre { Id = 2, Name = "Adventure", CreatedAt = DateTime.UtcNow },
            new Genre { Id = 3, Name = "Comedy", CreatedAt = DateTime.UtcNow },
            new Genre { Id = 4, Name = "Drama", CreatedAt = DateTime.UtcNow },
            new Genre { Id = 5, Name = "Horror", CreatedAt = DateTime.UtcNow }
            );
            modelBuilder.Entity<Director>().HasData(
            new Director { DirectorId = 1, Name = "Christopher Nolan" },
            new Director { DirectorId = 2, Name = "Peter Jackson" },
            new Director { DirectorId = 3, Name = "Taika Waititi" },
            new Director { DirectorId = 4, Name = "Greta Gerwig" },
            new Director { DirectorId = 5, Name = "James Wan" }
            );
            modelBuilder.Entity<Actor>().HasData(
            new Actor { Id = 1, Name = "Leonardo DiCaprio" },
            new Actor { Id = 2, Name = "Scarlett Johansson" },
            new Actor { Id = 3, Name = "Dwayne Johnson" },
            new Actor { Id = 4, Name = "Emma Stone" },
            new Actor { Id = 5, Name = "Daniel Kaluuya" }
            );
            modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
            Id = 1,
            Title = "The Final Stand",
            Description = "A gripping tale of survival in a post-apocalyptic world.",
            ReleaseYear = 2020,
            Duration = 130,
            GenreId = 1, // Action
            DirectorId = 1,
            CreatedAt = DateTime.UtcNow
            },
            new Movie
            {
            Id = 2,
            Title = "Quest for Glory",
            Description = "An epic adventure of a young hero seeking his destiny.",
            ReleaseYear = 2018,
            Duration = 145,
            GenreId = 2, // Adventure
            DirectorId = 2,
            CreatedAt = DateTime.UtcNow
            },
            new Movie
            {
            Id = 3,
            Title = "Laugh Out Loud",
            Description = "A hilarious comedy about friends navigating adulthood.",
            ReleaseYear = 2022,
            Duration = 95,
            GenreId = 3, // Comedy
            DirectorId = 3,
            CreatedAt = DateTime.UtcNow
            },
            new Movie
            {
            Id = 4,
            Title = "Tears of Tomorrow",
            Description = "A touching drama exploring the complexities of human emotions.",
            ReleaseYear = 2019,
            Duration = 120,
            GenreId = 4, // Drama
            DirectorId = 4,
            CreatedAt = DateTime.UtcNow
            },
            new Movie
            {
            Id = 5,
            Title = "Nightfall",
            Description = "A spine-chilling horror film set in an abandoned asylum.",
            ReleaseYear = 2021,
            Duration = 110,
            GenreId = 5, // Horror
            DirectorId = 5,
            CreatedAt = DateTime.UtcNow
            }
            );
            modelBuilder.Entity<MovieActor>().HasData(
            new MovieActor { MovieId = 1, ActorId = 1 }, // The Final Stand - Leonardo DiCaprio
            new MovieActor { MovieId = 1, ActorId = 2 }, // The Final Stand - Scarlett Johansson
            
            new MovieActor { MovieId = 2, ActorId = 3 }, // Quest for Glory - Dwayne Johnson
            new MovieActor { MovieId = 2, ActorId = 4 }, // Quest for Glory - Emma Stone
            
            new MovieActor { MovieId = 3, ActorId = 4 }, // Laugh Out Loud - Emma Stone
            new MovieActor { MovieId = 3, ActorId = 5 }, // Laugh Out Loud - Daniel Kaluuya
            
            new MovieActor { MovieId = 4, ActorId = 1 }, // Tears of Tomorrow - Leonardo DiCaprio
            new MovieActor { MovieId = 4, ActorId = 5 }, // Tears of Tomorrow - Daniel Kaluuya
            
            new MovieActor { MovieId = 5, ActorId = 2 }, // Nightfall - Scarlett Johansson
            new MovieActor { MovieId = 5, ActorId = 3 }  // Nightfall - Dwayne Johnson
            );


        }
    }
}
