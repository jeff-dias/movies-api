namespace Movies.Application.DTOs
{
    public class TicketDto : EntityDto
    {
        public bool Paid { get; set; }
        public Guid ShowtimeId { get; set; }
        public ShowtimeDto? Showtime { get; set; }
        public ICollection<BookedSeatDto>? BookedSeats { get; set; }
    }
}
