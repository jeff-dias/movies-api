using Microsoft.Extensions.DependencyInjection;
using Movies.Domain.Entities;
using Movies.Infra.Data.Context;

namespace Movies.Infra.IoC.Seed
{
    public static class SampleData
    {
        public static void Seed(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
            context.Database.EnsureCreated();

            var auditorium1 = new Auditorium();
            var seatsAuditorium1 = GenerateSeats(auditorium1.Id, 28, 22);

            context.Auditoriums.Add(auditorium1);
            context.Seats.AddRange(seatsAuditorium1);

            var auditorium2 = new Auditorium();
            var seatsAuditorium2 = GenerateSeats(auditorium2.Id, 21, 18);

            context.Auditoriums.Add(auditorium2);
            context.Seats.AddRange(seatsAuditorium2);

            var auditorium3 = new Auditorium();
            var seatsAuditorium3 = GenerateSeats(auditorium3.Id, 15, 21);

            context.Auditoriums.Add(auditorium3);
            context.Seats.AddRange(seatsAuditorium3);

            context.SaveChanges();
        }

        private static List<Seat> GenerateSeats(Guid auditoriumId, short rows, short seatsPerRow)
        {
            var seats = new List<Seat>();
            for (short r = 1; r <= rows; r++)
                for (short s = 1; s <= seatsPerRow; s++)
                    seats.Add(new Seat(r, s, auditoriumId));

            return seats;
        }
    }
}
