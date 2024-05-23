using Movies.Domain.Validation;

namespace Movies.Domain.Entities
{
    public sealed class Seat : Entity
    {
        public short Row { get; private set; }
        public short SeatNumber { get; private set; }
        public Guid AuditoriumId { get; private set; }
        public Auditorium? Auditorium { get; private set; }
        public ICollection<BookedSeat>? BookedSeats { get; private set; }

        public Seat(short row, short seatNumber, Guid auditoriumId) : base()
        {
            Validate(row, seatNumber, auditoriumId);

            Row = row;
            SeatNumber = seatNumber;
            AuditoriumId = auditoriumId;
        }

        private static void Validate(short row, short seatNumber, Guid auditoriumId)
        {
            DomainExceptionValidation.When(row <= 0, "Row must be greater than 0");
            DomainExceptionValidation.When(seatNumber <= 0, "Seat number must be greater than 0");
            DomainExceptionValidation.When(auditoriumId == Guid.Empty, "Auditorium is required");
        }
    }
}
