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
            new Genre { Id = 1, Name = "Action", CreatedAt = new DateTime(2023, 01, 15, 10, 30, 00, DateTimeKind.Utc) },
            new Genre { Id = 2, Name = "Adventure", CreatedAt = new DateTime(2022, 06, 20, 14, 45, 00, DateTimeKind.Utc) },
            new Genre { Id = 3, Name = "Comedy", CreatedAt = new DateTime(2021, 11, 05, 08, 15, 00, DateTimeKind.Utc) },
            new Genre { Id = 4, Name = "Drama", CreatedAt = new DateTime(2024, 03, 10, 19, 20, 00, DateTimeKind.Utc) },
            new Genre { Id = 5, Name = "Horror", CreatedAt = new DateTime(2020, 09, 25, 22, 50, 00, DateTimeKind.Utc) }
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
             new Movie { Id = 1, Title = "The Final Stand", Description = "In a world on the brink of collapse, a rogue soldier must unite unlikely allies to save humanity from its greatest threat.", CreatedAt = new DateTime(2023, 04, 01, 12, 00, 00, DateTimeKind.Utc), GenreId = 1, DirectorId = 1, ReleaseYear = 2020, Duration = 130 },
             new Movie { Id = 2, Title = "Quest for Glory", Description = "A young adventurer embarks on a perilous journey to reclaim a lost artifact, facing mythical creatures and ancient secrets.", CreatedAt = new DateTime(2022, 08, 15, 16, 30, 00, DateTimeKind.Utc), GenreId = 2, DirectorId = 2, ReleaseYear = 2018, Duration = 145 },
             new Movie { Id = 3, Title = "Laugh Out Loud", Description = "A series of hilarious misadventures unfolds when an awkward office worker tries to impress his coworkers during a company retreat.", CreatedAt = new DateTime(2021, 03, 22, 09, 10, 00, DateTimeKind.Utc), GenreId = 3, DirectorId = 3, ReleaseYear = 2022, Duration = 95 },
             new Movie { Id = 4, Title = "Tears of Tomorrow", Description = "In a dystopian future, two strangers form an unlikely bond as they fight to survive in a world devoid of hope.", CreatedAt = new DateTime(2024, 12, 05, 18, 40, 00, DateTimeKind.Utc), GenreId = 4, DirectorId = 4, ReleaseYear = 2019, Duration = 120 },
             new Movie { Id = 5, Title = "Nightfall", Description = "A small town is haunted by mysterious disappearances, and a group of friends must uncover the dark truth before it's too late.", CreatedAt = new DateTime(2020, 07, 30, 21, 05, 00, DateTimeKind.Utc), GenreId = 5, DirectorId = 5, ReleaseYear = 2021, Duration = 110 }
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
