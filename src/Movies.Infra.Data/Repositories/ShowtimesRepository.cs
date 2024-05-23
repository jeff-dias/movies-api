using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Repositories
{
    public class ShowtimesRepository : IShowtimesRepository
    {
        private readonly MoviesContext _context;

        public ShowtimesRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Showtime>> GetAllAsync(Expression<Func<Showtime, bool>> filter, CancellationToken cancel)
        {
            if (filter == null)
            {
                return await _context.Showtimes
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancel);
            }

            return await _context.Showtimes
                .Where(x => !x.IsDeleted)
                .Where(filter)
                .ToListAsync(cancel);
        }

        public async Task<Showtime> CreateAsync(Showtime showtime, CancellationToken cancel)
        {
            await _context.Showtimes.AddAsync(showtime, cancel);
            await _context.SaveChangesAsync(cancel);
            return showtime;
        }

        public async Task<Showtime> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            var showtimes = _context.Showtimes.ToList();
            return await _context.Showtimes
                .Include(x => x.Auditorium).ThenInclude(n => n.Seats)
                .Include(x => x.Tickets.Where(t => !t.IsDeleted)).ThenInclude(n => n.BookedSeats.Where(b => !b.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancel) ?? throw new ApplicationException($"Entity could not be found.");
        }
    }
}
