using MediatR;
using Movies.Application.Mediators.Showtimes.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Mediators.Showtimes.Handlers
{
    public class ShowtimeCreateCommandHandler : IRequestHandler<ShowtimeCreateCommand, Showtime>
    {
        private readonly IShowtimesRepository _showtimesRepository;

        public ShowtimeCreateCommandHandler(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        public async Task<Showtime> Handle(ShowtimeCreateCommand request, CancellationToken cancellationToken)
        {
            var showtime = new Showtime(request.SessionDate, request.AuditoriumId, request.ExternalMovieId);

            return await _showtimesRepository.CreateAsync(showtime, cancellationToken);
        }
    }
}
