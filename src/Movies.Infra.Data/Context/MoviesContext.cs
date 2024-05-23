using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Infra.Data.Context
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        { }

        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<BookedSeat> BookedSeats { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesContext).Assembly);
        }
    }
}
