using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface IAuditoriumsRepository
    {
        Task<Auditorium> GetByIdAsync(Guid id, CancellationToken cancel);
        Task<ICollection<Auditorium>> GetAsync(CancellationToken cancel);
    }
}
