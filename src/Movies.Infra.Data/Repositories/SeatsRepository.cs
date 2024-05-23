using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Repositories
{
    public class SeatsRepository : ISeatsRepository
    {
        private readonly MoviesContext _context;

        public SeatsRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Seat>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Seats
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
