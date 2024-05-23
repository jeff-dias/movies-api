using Movies.Application.DTOs;

namespace Movies.Application.Interfaces
{
    public interface IAuditoriumService
    {
        Task<AuditoriumDto> GetByIdAsync(Guid id, CancellationToken cancel);
        Task<ICollection<AuditoriumDto>> GetAsync(CancellationToken cancel);
    }
}
