using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface ITicketsRepository
    {
        Task<Ticket> ConfirmPaymentAsync(Guid id, CancellationToken cancel);
        Task<Ticket> CancelAsync(Guid id, CancellationToken cancel);
        Task<Ticket> CreateAsync(Ticket ticket, ICollection<BookedSeat> bookedSeats, CancellationToken cancel);
        Task<Ticket> GetEnrichedByIdAsync(Guid id, CancellationToken cancel);
    }
}
