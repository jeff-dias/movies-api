using MediatR;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Tickets.Handlers
{
    public class TicketConfirmPaymentCommandHandler : IRequestHandler<TicketConfirmPaymentCommand, Ticket>
    {
        private readonly ITicketsRepository _ticketsRepository;

        public TicketConfirmPaymentCommandHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public async Task<Ticket> Handle(TicketConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            return await _ticketsRepository.ConfirmPaymentAsync(request.Id, cancellationToken);
        }
    }
}
