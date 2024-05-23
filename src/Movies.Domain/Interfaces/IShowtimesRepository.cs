using System.Linq.Expressions;
using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface IShowtimesRepository
    {
        Task<Showtime> CreateAsync(Showtime showtimeEntity, CancellationToken cancel);
        Task<ICollection<Showtime>> GetAllAsync(Expression<Func<Showtime, bool>> filter, CancellationToken cancel);
        Task<Showtime> GetEnrichedByIdAsync(Guid id, CancellationToken cancel);
    }
}
