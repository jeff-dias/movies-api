using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface ISeatsRepository
    {
        Task<ICollection<Seat>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken);
    }
}
