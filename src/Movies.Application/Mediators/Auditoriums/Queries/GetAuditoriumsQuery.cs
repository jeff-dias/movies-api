using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Auditoriums.Queries
{
    public class GetAuditoriumsQuery : IRequest<ICollection<Auditorium>>
    {
    }
}
