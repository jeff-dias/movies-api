namespace Movies.Application.DTOs
{
    public class AuditoriumDto : EntityDto
    {
        public ICollection<ShowtimeDto> Showtimes { get; set; }
        public ICollection<SeatDto> Seats { get; set; }
    }
}
