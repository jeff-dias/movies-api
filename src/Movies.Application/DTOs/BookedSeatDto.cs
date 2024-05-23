namespace Movies.Application.DTOs
{
    public class BookedSeatDto : EntityDto
    {
        public Guid TicketId { get; set; }
        public TicketDto? Ticket { get; set; }
        public Guid SeatId { get; set; }
        public SeatDto? Seat { get; set; }
    }
}
