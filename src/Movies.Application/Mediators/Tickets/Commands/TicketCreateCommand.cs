using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Tickets.Commands
{
    public class TicketCreateCommand : IRequest<Ticket>
    {
        public Guid ShowtimeId { get; set; }
        public List<BookedSeat> BookedSeats { get; set; }
        public TicketCreateCommand(Guid showtimeId, List<BookedSeat> bookedSeats)
        {
            ShowtimeId = showtimeId;
            BookedSeats = bookedSeats;
        }
    }
}
