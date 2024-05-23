using Movies.Application.DTOs;

namespace Movies.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto> GetEnrichedByIdAsync(Guid id, CancellationToken cancel);
        Task<TicketDto> CreateAsync(TicketDto ticket, CancellationToken cancel);
        Task<TicketDto> ConfirmPaymentAsync(Guid id, CancellationToken cancel);
        Task<TicketDto> CancelAsync(Guid id, CancellationToken cancel);
    }
}
