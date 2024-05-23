using AutoMapper;
using MediatR;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;
using Movies.Application.Mediators.Showtimes.Commands;
using Movies.Application.Mediators.Showtimes.Queries;

namespace Movies.Application.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IExternalMoviesService _externalMoviesService;

        public ShowtimeService(IMediator mediator, IMapper mapper, IExternalMoviesService externalMoviesService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _externalMoviesService = externalMoviesService;
        }

        public async Task<ShowtimeDto> CreateAsync(ShowtimeDto showtime, CancellationToken cancel)
        {
            var showtimeCreateCommand = _mapper.Map<ShowtimeCreateCommand>(showtime);
            var showtimeCreated = await _mediator.Send(showtimeCreateCommand, cancel);
            return _mapper.Map<ShowtimeDto>(showtimeCreated);
        }

        public async Task<ICollection<ShowtimeDto>> GetAsync(CancellationToken cancel)
        {
            var showtimes = await _mediator.Send(new GetShowtimesQuery(), cancel);
            var showtimeDtos = _mapper.Map<ICollection<ShowtimeDto>>(showtimes);

            var movies = await _externalMoviesService.GetAsync(cancel);
            showtimeDtos.ToList().ForEach(showtimeDto => showtimeDto.Movie = movies?.FirstOrDefault(movie => movie.Id == showtimeDto.ExternalMovieId));

            return showtimeDtos;
        }

        public async Task<ShowtimeDto> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            var getShowtimeEnrichedByIduery = new GetShowtimeEnrichedByIdQuery(id);
            var showtime = await _mediator.Send(getShowtimeEnrichedByIduery, cancel);

            var showtimeDto = _mapper.Map<ShowtimeDto>(showtime);
            showtimeDto.ValidateReserves();
            showtimeDto.Movie = await _externalMoviesService.GetByIdAsync(showtime.ExternalMovieId, cancel);

            return showtimeDto;
        }
    }
}
