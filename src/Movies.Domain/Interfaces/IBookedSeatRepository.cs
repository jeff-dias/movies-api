using Movies.Domain.Entities;

namespace Movies.Domain.Interfaces
{
    public interface IBookedSeatsRepository
    {
        Task<BookedSeat?> GetBySeatIdAndShowtimeIdAsync(Guid id, Guid showtimeId, CancellationToken cancellationToken);
        Task<BookedSeat?> UpdateAsync(BookedSeat bookedSeat, CancellationToken cancellationToken);
    }
}
