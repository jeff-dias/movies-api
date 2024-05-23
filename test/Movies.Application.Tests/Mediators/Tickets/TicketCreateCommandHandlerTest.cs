using Moq;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Tickets.Handlers;
using Movies.Application.Mediators.Validation;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Tickets
{
    public class TicketCreateCommandHandlerTest
    {
        private readonly Mock<ITicketsRepository> _repository;
        private readonly Mock<IBookedSeatsRepository> _bookedSeatsRepository;
        private readonly Mock<ISeatsRepository> _seatsRepository;

        public TicketCreateCommandHandlerTest()
        {
            _repository = new Mock<ITicketsRepository>();
            _bookedSeatsRepository = new Mock<IBookedSeatsRepository>();
            _seatsRepository = new Mock<ISeatsRepository>();
        }

        [Fact]
        public async void TicketCreateCommand_WhenBookedSeatsIsEmpty_ReturnBusinessException()
        {
            //Arrange
            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), new List<BookedSeat>());

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var ex = await Record.ExceptionAsync(() => handler.Handle(ticketCommand, new CancellationToken()));

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsType<BusinessExceptionValidation>(ex);
            Assert.Equal("Booked seats is required.", ex.Message);
        }

        [Fact]
        public async void TicketCreateCommand_WhenAnyBookedSeatIdIsNotFoundOnDb_ReturnBusinessException()
        {
            //Arrange
            var bookedSeats = new List<BookedSeat>{
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid())
            };

            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), bookedSeats);

            _seatsRepository.Setup(x => x.GetByIdsAsync(It.IsAny<ICollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Seat>());

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var ex = await Record.ExceptionAsync(() => handler.Handle(ticketCommand, new CancellationToken()));

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsType<BusinessExceptionValidation>(ex);
            Assert.Equal("Seats not found.", ex.Message);
        }

        [Theory]
        [InlineData(5, 8)]
        [InlineData(5, 13)]
        [InlineData(2, 9)]
        public async void TicketCreateCommand_WhenBookedSeatsAreNotContiguous_ReturnBusinessException(short seatRow, short seatNumber)
        {
            //Arrange
            var bookedSeats = new List<BookedSeat>{
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid())
            };

            var seats = new List<Seat>
            {
                new Seat(5,10, Guid.NewGuid()),
                new Seat(5,11, Guid.NewGuid()),
                new Seat(seatRow, seatNumber, Guid.NewGuid())
            };

            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), bookedSeats);

            _seatsRepository.Setup(x => x.GetByIdsAsync(It.IsAny<ICollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seats);

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var ex = await Record.ExceptionAsync(() => handler.Handle(ticketCommand, new CancellationToken()));

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsType<BusinessExceptionValidation>(ex);
            Assert.Equal("Seats must be contiguous.", ex.Message);
        }

        [Fact]
        public async void TicketCreateCommand_WhenBookedSeatsIsAlreadyPaid_ReturnBusinessException()
        {
            //Arrange
            var bookedSeats = new List<BookedSeat>{
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid())
            };

            var seats = new List<Seat>
            {
                new Seat(5, 10, Guid.NewGuid()),
                new Seat(5, 11, Guid.NewGuid()),
                new Seat(5, 12, Guid.NewGuid())
            };

            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), bookedSeats);
            var ticket = new Ticket(Guid.NewGuid());
            ticket.ConfirmPayment();
            var bookedSeatAlreadyPaid = new BookedSeat(Guid.NewGuid())
            {
                Ticket = ticket
            };

            _seatsRepository.Setup(x => x.GetByIdsAsync(It.IsAny<ICollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seats);

            _bookedSeatsRepository.Setup(x => x.GetBySeatIdAndShowtimeIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookedSeatAlreadyPaid);

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var ex = await Record.ExceptionAsync(() => handler.Handle(ticketCommand, new CancellationToken()));

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsType<BusinessExceptionValidation>(ex);
            Assert.Equal($"Seat {bookedSeats.First().SeatId} is not available. It is already paid.", ex.Message);
        }

        [Fact]
        public async void TicketCreateCommand_WhenBookedSeatsIsAlreadyBooked_ReturnBusinessException()
        {
            //Arrange
            var bookedSeats = new List<BookedSeat>{
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid())
            };

            var seats = new List<Seat>
            {
                new Seat(5, 10, Guid.NewGuid()),
                new Seat(5, 11, Guid.NewGuid()),
                new Seat(5, 12, Guid.NewGuid())
            };

            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), bookedSeats);
            var ticket = new Ticket(Guid.NewGuid());
            var bookedSeatAlreadyPaid = new BookedSeat(Guid.NewGuid())
            {
                Ticket = ticket
            };

            _seatsRepository.Setup(x => x.GetByIdsAsync(It.IsAny<ICollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seats);

            _bookedSeatsRepository.Setup(x => x.GetBySeatIdAndShowtimeIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookedSeatAlreadyPaid);

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var ex = await Record.ExceptionAsync(() => handler.Handle(ticketCommand, new CancellationToken()));

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsType<BusinessExceptionValidation>(ex);
            Assert.Equal($"Seat {bookedSeats.First().SeatId} is already booked. Its reservation will expire in 10 minutes.", ex.Message);
        }

        [Fact]
        public async void TicketCreateCommand_WhenBookedSeatsAreAvailable_ReturnOK()
        {
            //Arrange
            var bookedSeats = new List<BookedSeat>{
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid()),
                new BookedSeat(Guid.NewGuid())
            };

            var seats = new List<Seat>
            {
                new Seat(5, 10, Guid.NewGuid()),
                new Seat(5, 11, Guid.NewGuid()),
                new Seat(5, 12, Guid.NewGuid())
            };

            var ticketCommand = new TicketCreateCommand(Guid.NewGuid(), bookedSeats);
            var ticket = new Ticket(Guid.NewGuid());
            var bookedSeatAlreadyPaid = new BookedSeat(Guid.NewGuid())
            {
                Ticket = ticket
            };

            _seatsRepository.Setup(x => x.GetByIdsAsync(It.IsAny<ICollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seats);

            _repository.Setup(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            var handler = new TicketCreateCommandHandler(_repository.Object, _bookedSeatsRepository.Object, _seatsRepository.Object);

            //Act
            var result = await handler.Handle(ticketCommand, new CancellationToken());

            //Assert
            _repository.Verify(x => x.CreateAsync(It.IsAny<Ticket>(), It.IsAny<ICollection<BookedSeat>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
        }
    }
}
