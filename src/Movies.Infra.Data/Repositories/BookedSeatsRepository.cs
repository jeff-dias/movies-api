using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Repositories
{
    public class BookedSeatsRepository : IBookedSeatsRepository
    {
        private readonly MoviesContext _context;

        public BookedSeatsRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<BookedSeat?> GetBySeatIdAndShowtimeIdAsync(Guid seatId, Guid showtimeId, CancellationToken cancellationToken)
        {
            return await _context.BookedSeats
                .Include(x => x.Seat)
                .Include(x => x.Ticket)
                .FirstOrDefaultAsync(x => x.SeatId == seatId
                                    && x.Ticket.ShowtimeId == showtimeId
                                    && !x.IsDeleted
                                    && (x.Ticket.Paid
                                    || x.Ticket.BookingExpiresAt >= DateTime.Now), cancellationToken);
        }

        public async Task<BookedSeat?> UpdateAsync(BookedSeat bookedSeat, CancellationToken cancellationToken)
        {
            _context.Update(bookedSeat);
            await _context.SaveChangesAsync(cancellationToken);
            return bookedSeat;
        }
    }
}
