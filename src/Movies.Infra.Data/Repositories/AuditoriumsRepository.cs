using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly MoviesContext _context;

        public AuditoriumsRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Auditorium>> GetAsync(CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancel);
        }

        public async Task<Auditorium> GetByIdAsync(Guid id, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancel) ?? throw new ApplicationException($"Entity could not be found.");
        }
    }
}
