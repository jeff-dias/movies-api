using Movies.Domain.Entities;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class TicketsRepositoryTest : BaseRepositoryTest
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
        public async Task GetEnrichedById_WhenDataExists_ReturnOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);

            //Act & Assert
            var repository = new TicketsRepository(context);

            var result = await repository.GetEnrichedByIdAsync(Ticket.Id, CancellationToken.None);
            Assert.Equal(Ticket, result);
        }

        [Fact]
        public async Task GetEnrichedById_WhenDataDoesNotExist_ReturnApplicationException()
        {
            //Arrange
            using var context = new MoviesContext(_options);

            //Act & Assert
            var repository = new TicketsRepository(context);

            await Assert.ThrowsAsync<ApplicationException>(() => repository.GetEnrichedByIdAsync(Guid.NewGuid(), CancellationToken.None));
        }

        [Fact]
        public async Task Create_WhenDataIsValid_ReturnCreatedOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);

            var showtime = new Showtime(DateTime.Now.AddDays(1), Auditorium.Id, "external-movie-id");
            var ticket = new Ticket(showtime.Id);
            var bookedSeat1 = new BookedSeat(Seat1.Id);
            bookedSeat1.ConfirmBooking(ticket.Id);
            var bookedSeat2 = new BookedSeat(Seat2.Id);
            bookedSeat2.ConfirmBooking(ticket.Id);
            var bookedSeat3 = new BookedSeat(Seat3.Id);
            bookedSeat3.ConfirmBooking(ticket.Id);
            var bookedSeats = new List<BookedSeat> { bookedSeat1, bookedSeat2, bookedSeat3 };

            context.Add(showtime);
            context.SaveChanges();

            //Act & Assert
            var repository = new TicketsRepository(context);
            var bookedSeatsRepository = new BookedSeatsRepository(context);

            var result = await repository.CreateAsync(ticket, bookedSeats, CancellationToken.None);
            Assert.Equal(ticket, result);

            var createdBookedSeat1 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat1.Id, showtime.Id, CancellationToken.None);
            Assert.Equal(bookedSeat1, createdBookedSeat1);

            var createdBookedSeat2 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat2.Id, showtime.Id, CancellationToken.None);
            Assert.Equal(bookedSeat2, createdBookedSeat2);

            var createdBookedSeat3 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat3.Id, showtime.Id, CancellationToken.None);
            Assert.Equal(bookedSeat3, createdBookedSeat3);
        }

        [Fact]
        public async Task ConfirmPayment_WhenDataExists_ReturnOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);

            //Act & Assert
            var repository = new TicketsRepository(context);

            var result = await repository.ConfirmPaymentAsync(Ticket.Id, CancellationToken.None);
            Assert.True(result.Paid);
        }

        [Fact]
        public async Task ConfirmPayment_WhenDataDoesNotExist_ReturnApplicationException()
        {
            //Arrange
            using var context = new MoviesContext(_options);

            //Act & Assert
            var repository = new TicketsRepository(context);

            await Assert.ThrowsAsync<ApplicationException>(() => repository.ConfirmPaymentAsync(Guid.NewGuid(), CancellationToken.None));
        }

        [Fact]
        public async Task Cancel_WhenDataExists_ReturnOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            DbArrange(context);

            //Act & Assert
            var repository = new TicketsRepository(context);
            var bookedSeatsRepository = new BookedSeatsRepository(context);

            var result = await repository.CancelAsync(Ticket.Id, CancellationToken.None);
            Assert.True(result.IsDeleted);

            var bookedSeat1 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat1.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(bookedSeat1);

            var bookedSeat2 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat2.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(bookedSeat2);

            var bookedSeat3 = await bookedSeatsRepository.GetBySeatIdAndShowtimeIdAsync(Seat3.Id, Showtime.Id, CancellationToken.None);
            Assert.Null(bookedSeat3);
        }
    }
}
