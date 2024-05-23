using Movies.Domain.Entities;
using MediatR;

namespace Movies.Application.Mediators.Auditoriums.Queries
{
    public class GetAuditoriumByIdQuery : IRequest<Auditorium>
    {
        public Guid Id { get; set; }

        public GetAuditoriumByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
