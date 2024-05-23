using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly MoviesContext _context;

        public TicketsRepository(MoviesContext context)
        {
            _context = context;
        }

        public async Task<Ticket> GetEnrichedByIdAsync(Guid id, CancellationToken cancel)
        {
            return await _context.Tickets
                .Include(x => x.Showtime).ThenInclude(n => n.Auditorium)
                .Include(x => x.BookedSeats)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancel)
                ?? throw new ApplicationException($"Entity could not be found.");
        }

        public async Task<Ticket> CreateAsync(Ticket ticket, ICollection<BookedSeat> bookedSeats, CancellationToken cancel)
        {
            _context.BookedSeats.AddRange(bookedSeats);

            await _context.Tickets.AddAsync(ticket);

            await _context.SaveChangesAsync(cancel);

            return ticket;
        }

        public async Task<Ticket> ConfirmPaymentAsync(Guid id, CancellationToken cancel)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancel)
                ?? throw new ApplicationException($"Entity could not be found.");

            ticket.ConfirmPayment();
            _context.Update(ticket);
            await _context.SaveChangesAsync(cancel);
            return ticket;
        }

        public async Task<Ticket> CancelAsync(Guid id, CancellationToken cancel)
        {
            var ticket = await _context.Tickets
                .Include(x => x.BookedSeats)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancel)
                ?? throw new ApplicationException($"Entity could not be found.");

            foreach (var seat in ticket.BookedSeats)
            {
                seat.CancelBooking();
                _context.Update(seat);
            }

            ticket.Cancel();
            _context.Update(ticket);
            await _context.SaveChangesAsync(cancel);

            return ticket;
        }
    }
}
