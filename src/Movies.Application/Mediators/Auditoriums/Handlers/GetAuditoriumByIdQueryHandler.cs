using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Movies.Application.Extensions;
using Movies.Application.Mediators.Auditoriums.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Auditoriums.Handlers
{
    public class GetAuditoriumByIdQueryHandler : IRequestHandler<GetAuditoriumByIdQuery, Auditorium>
    {
        private readonly IAuditoriumsRepository _repository;

        public GetAuditoriumByIdQueryHandler(IAuditoriumsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Auditorium> Handle(GetAuditoriumByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
