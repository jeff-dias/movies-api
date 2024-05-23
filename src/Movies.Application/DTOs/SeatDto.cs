namespace Movies.Application.DTOs
{
    public class SeatDto : EntityDto
    {
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public Guid AuditoriumId { get; set; }
        public AuditoriumDto? Auditorium { get; set; }
        public bool IsReserved { get; set; }
        public string FriendlyCode => $"{IntToLetters(Row)}-{SeatNumber}";
        public ICollection<BookedSeatDto>? BookedSeats { get; private set; }

        private static string IntToLetters(short value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }
    }
}
