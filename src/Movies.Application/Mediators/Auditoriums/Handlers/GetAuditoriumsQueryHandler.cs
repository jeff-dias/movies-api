using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Movies.Application.Extensions;
using Movies.Application.Mediators.Auditoriums.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Auditoriums.Handlers
{
    public class GetAuditoriumsQueryHandler : IRequestHandler<GetAuditoriumsQuery, ICollection<Auditorium>>
    {
        private readonly IAuditoriumsRepository _repository;

        public GetAuditoriumsQueryHandler(IAuditoriumsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<Auditorium>> Handle(GetAuditoriumsQuery _, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(cancellationToken);
        }
    }
}
