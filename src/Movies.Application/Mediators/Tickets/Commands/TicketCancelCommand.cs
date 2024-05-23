using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Tickets.Commands
{
    public class TicketCancelCommand : IRequest<Ticket>
    {
        public Guid Id { get; set; }
        public TicketCancelCommand(Guid id)
        {
            Id = id;
        }
    }
}
