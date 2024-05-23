using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Tickets.Queries
{
    public class GetTicketEnrichedByIdQuery : IRequest<Ticket>
    {
        public Guid Id { get; set; }
        public GetTicketEnrichedByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
