using MediatR;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Validation;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Tickets.Handlers
{
    public class TicketCreateCommandHandler : IRequestHandler<TicketCreateCommand, Ticket>
    {
        private readonly ITicketsRepository _ticketRepository;
        private readonly IBookedSeatsRepository _bookedSeatRepository;
        private readonly ISeatsRepository _seatsRepository;

        public TicketCreateCommandHandler(
            ITicketsRepository ticketsRepository,
            IBookedSeatsRepository bookedSeatRepository,
            ISeatsRepository seatsRepository)
        {
            _ticketRepository = ticketsRepository;
            _bookedSeatRepository = bookedSeatRepository;
            _seatsRepository = seatsRepository;
        }

        public async Task<Ticket> Handle(TicketCreateCommand request, CancellationToken cancellationToken)
        {
            BusinessExceptionValidation.When(request.BookedSeats?.Count == 0, "Booked seats is required.");

            var ticket = new Ticket(request.ShowtimeId);
            await ValidateSeats(request, cancellationToken);
            var bookedSeats = await ValidateBookedSeats(request, ticket.Id, cancellationToken);

            return await _ticketRepository.CreateAsync(ticket, bookedSeats, cancellationToken);
        }

        private async Task ValidateSeats(TicketCreateCommand request, CancellationToken cancellationToken)
        {
            var seatIds = request.BookedSeats.Select(x => x.SeatId).ToList();
            var seats = await _seatsRepository.GetByIdsAsync(seatIds, cancellationToken);

            BusinessExceptionValidation.When(request.BookedSeats.Count != seats.Count, $"Seats not found.");

            var seatRow = 0;
            var seatNumber = 0;
            var isFirst = true;

            foreach (var seat in seats)
            {
                if (isFirst)
                {
                    seatRow = seat.Row;
                    seatNumber = seat.SeatNumber;
                    isFirst = false;
                    continue;
                }

                BusinessExceptionValidation.When(seatRow != seat.Row || (seatNumber - seat.SeatNumber) > 1 || (seatNumber - seat.SeatNumber) < -1,
                        $"Seats must be contiguous.");

                seatRow = seat.Row;
                seatNumber = seat.SeatNumber;
            }
        }

        private async Task<List<BookedSeat>> ValidateBookedSeats(TicketCreateCommand request, Guid ticketId, CancellationToken cancellationToken)
        {
            var bookedSeats = new List<BookedSeat>();

            foreach (var bookedSeat in request.BookedSeats)
            {
                var seatAlreadyBooked = await _bookedSeatRepository.GetBySeatIdAndShowtimeIdAsync(bookedSeat.SeatId, request.ShowtimeId, cancellationToken);

                if (seatAlreadyBooked != null)
                {
                    BusinessExceptionValidation.When(seatAlreadyBooked.Ticket?.Paid ?? false,
                                $"Seat {bookedSeat.SeatId} is not available. It is already paid.");

                    var minutesToExpire = (seatAlreadyBooked.Ticket?.BookingExpiresAt - DateTime.Now)?.TotalMinutes ?? 10;

                    BusinessExceptionValidation.When(seatAlreadyBooked.Ticket?.BookingExpiresAt >= DateTime.Now,
                                $"Seat {bookedSeat.SeatId} is already booked. Its reservation will expire in {Math.Round(minutesToExpire)} minutes.");

                    await _ticketRepository.CancelAsync(seatAlreadyBooked.TicketId, cancellationToken);
                }

                var bookedSeatEntity = new BookedSeat(bookedSeat.SeatId);
                bookedSeatEntity.ConfirmBooking(ticketId);
                bookedSeats.Add(bookedSeatEntity);
            }

            return bookedSeats;
        }
    }
}
