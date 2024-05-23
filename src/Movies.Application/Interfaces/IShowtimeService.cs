using Movies.Application.DTOs;

namespace Movies.Application.Interfaces
{
    public interface IShowtimeService
    {
        Task<ICollection<ShowtimeDto>> GetAsync(CancellationToken cancel);
        Task<ShowtimeDto> GetEnrichedByIdAsync(Guid id, CancellationToken cancel);
        Task<ShowtimeDto> CreateAsync(ShowtimeDto showtime, CancellationToken cancel);
    }
}
