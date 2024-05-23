using Movies.Domain.Entities;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class BookedSeatRepositoryTest : BaseRepositoryTest
    {
        private Auditorium? Auditorium;
        private Seat? Seat1;
        private Seat? Seat2;
        private Seat? Seat3;
        private Showtime? Showtime;
        private Ticket? Ticket;
        private BookedSeat? BookedSeat1;
        private BookedSeat? BookedSeat2;
        private BookedSeat? BookedSeat3;

        private void DbArrange(MoviesContext context)
        {
            Auditorium = new Auditorium();
            Seat1 = new Seat(1, 1, Auditorium.Id);
            Seat2 = new Seat(1, 2, Auditorium.Id);
            Seat3 = new Seat(1, 3, Auditorium.Id);
            Showtime = new Showtime(DateTime.Now.AddDays(1), Auditorium.Id, "external-movie-id");
            Ticket = new Ticket(Showtime.Id);
            BookedSeat1 = new BookedSeat(Seat1.Id);
            BookedSeat1.ConfirmBooking(Ticket.Id);
            BookedSeat2 = new BookedSeat(Seat2.Id);
            BookedSeat2.ConfirmBooking(Ticket.Id);
            BookedSeat3 = new BookedSeat(Seat3.Id);
            BookedSeat3.ConfirmBooking(Ticket.Id);

            context.Auditoriums.Add(Auditorium);
            context.Seats.Add(Seat1);
            context.Seats.Add(Seat2);
            context.Seats.Add(Seat3);
            context.Showtimes.Add(Showtime);
            context.Tickets.Add(Ticket);
            context.BookedSeats.Add(BookedSeat1);
            context.BookedSeats.Add(BookedSeat2);
            context.BookedSeats.Add(BookedSeat3);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetBySeatIdAndShowtimeId_WhenDbHasData_ReturnDataOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);

            //Act & Assert
            var repository = new BookedSeatsRepository(context);

            var result1 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat1.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat1.Id, result1.Id);

            var result2 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat2.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat2.Id, result2.Id);

            var result3 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat3.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat3.Id, result3.Id);
        }

        [Fact]
        public async Task GetBySeatIdAndShowtimeId_WhenTheBookingsWereCanceled_ReturnNull()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);
            BookedSeat1.CancelBooking();
            BookedSeat2.CancelBooking();
            BookedSeat3.CancelBooking();
            context.BookedSeats.UpdateRange(BookedSeat1, BookedSeat2, BookedSeat3);
            context.SaveChanges();

            //Act & Assert
            var repository = new BookedSeatsRepository(context);
            var result1 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat1.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(result1);

            var result2 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat2.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(result2);

            var result3 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat3.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(result3);
        }

        [Fact]
        public async Task GetBySeatIdAndShowtimeId_WhenTheBookingsWerePaid_ReturnDataOKToBlockNewBookings()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);
            Ticket.ConfirmPayment();
            context.Tickets.Update(Ticket);
            context.SaveChanges();

            //Act & Assert
            var repository = new BookedSeatsRepository(context);

            var result1 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat1.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat1.Id, result1.Id);

            var result2 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat2.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat2.Id, result2.Id);

            var result3 = await repository.GetBySeatIdAndShowtimeIdAsync(Seat3.Id, Showtime.Id, CancellationToken.None);
            Assert.Equal(BookedSeat3.Id, result3.Id);
        }
    }
}
