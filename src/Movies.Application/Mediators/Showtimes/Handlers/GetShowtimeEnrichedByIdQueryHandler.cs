using MediatR;
using Movies.Application.Mediators.Showtimes.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Showtimes.Handlers
{
    public class GetShowtimeEnrichedByIdQueryHandler : IRequestHandler<GetShowtimeEnrichedByIdQuery, Showtime>
    {
        private readonly IShowtimesRepository _showtimesRepository;

        public GetShowtimeEnrichedByIdQueryHandler(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        public async Task<Showtime> Handle(GetShowtimeEnrichedByIdQuery request, CancellationToken cancellationToken)
        {
            return await _showtimesRepository.GetEnrichedByIdAsync(request.Id, cancellationToken);
        }
    }
}
