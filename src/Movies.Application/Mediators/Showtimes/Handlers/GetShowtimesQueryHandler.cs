using MediatR;
using Movies.Application.Mediators.Showtimes.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Showtimes.Handlers
{
    public class GetShowtimesQueryHandler : IRequestHandler<GetShowtimesQuery, ICollection<Showtime>>
    {
        private readonly IShowtimesRepository _showtimesRepository;

        public GetShowtimesQueryHandler(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        public async Task<ICollection<Showtime>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            return await _showtimesRepository.GetAllAsync(x => true, cancellationToken);
        }
    }
}
