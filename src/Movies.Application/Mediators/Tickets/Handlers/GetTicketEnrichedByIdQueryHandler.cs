using MediatR;
using Movies.Application.Mediators.Tickets.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Tickets.Handlers
{
    public class GetTicketEnrichedByIdQueryHandler : IRequestHandler<GetTicketEnrichedByIdQuery, Ticket>
    {
        private readonly ITicketsRepository _ticketsRepository;

        public GetTicketEnrichedByIdQueryHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public async Task<Ticket> Handle(GetTicketEnrichedByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ticketsRepository.GetEnrichedByIdAsync(request.Id, cancellationToken);
        }
    }
}
