using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Tickets.Commands
{
    public class TicketConfirmPaymentCommand : IRequest<Ticket>
    {
        public Guid Id { get; set; }
        public TicketConfirmPaymentCommand(Guid id)
        {
            Id = id;
        }
    }
}
