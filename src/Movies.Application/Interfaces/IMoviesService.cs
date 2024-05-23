using Movies.Application.DTOs;

namespace Movies.Application.Interfaces
{
    public interface IExternalMoviesService
    {
        Task<ICollection<ExternalMovieDto>> GetAsync(CancellationToken cancel);
        Task<ExternalMovieDto> GetByIdAsync(string id, CancellationToken cancel);
    }
}
