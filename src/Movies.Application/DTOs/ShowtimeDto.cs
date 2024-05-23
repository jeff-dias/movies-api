namespace Movies.Application.DTOs
{
    public class ShowtimeDto : EntityDto
    {
        public ShowtimeDto()
        {
        }

        public DateTime SessionDate { get; set; }
        public string ExternalMovieId { get; set; }
        public ExternalMovieDto? Movie { get; set; }
        public Guid AuditoriumId { get; set; }
        public AuditoriumDto? Auditorium { get; set; }
        public ICollection<TicketDto>? Tickets { get; set; }
        public string FriendlySessionDate => SessionDate.ToString("dd/MM/yyyy HH:mm");

        public void ValidateReserves()
        {
            if (Auditorium == null || Tickets == null)
            {
                return;
            }

            var reservedSeats = new List<SeatDto>();
            foreach (var seat in Auditorium.Seats)
            {
                seat.IsReserved = Tickets?.Any(y => y.BookedSeats?.Any(z => z.SeatId == seat.Id) ?? false) ?? false;
                reservedSeats.Add(seat);
            }

            Auditorium.Seats = reservedSeats;
        }
    }
}
