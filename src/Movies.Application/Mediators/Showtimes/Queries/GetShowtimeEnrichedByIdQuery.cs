using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Showtimes.Queries
{
    public class GetShowtimeEnrichedByIdQuery : IRequest<Showtime>
    {
        public Guid Id { get; set; }
        public GetShowtimeEnrichedByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
