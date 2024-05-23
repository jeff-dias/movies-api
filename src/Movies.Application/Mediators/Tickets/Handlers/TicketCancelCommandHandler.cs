using MediatR;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Tickets.Handlers
{
    public class TicketCancelCommandHandler : IRequestHandler<TicketCancelCommand, Ticket>
    {
        private readonly ITicketsRepository _ticketsRepository;

        public TicketCancelCommandHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public async Task<Ticket> Handle(TicketCancelCommand request, CancellationToken cancellationToken)
        {
            return await _ticketsRepository.CancelAsync(request.Id, cancellationToken);
        }
    }
}
