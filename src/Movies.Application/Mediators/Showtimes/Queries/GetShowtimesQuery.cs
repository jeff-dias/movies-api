using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Showtimes.Queries
{
    public class GetShowtimesQuery : IRequest<ICollection<Showtime>>
    {
    }
}
