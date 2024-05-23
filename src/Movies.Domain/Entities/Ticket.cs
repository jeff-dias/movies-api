using Movies.Domain.Validation;

namespace Movies.Domain.Entities
{
    public sealed class Ticket : Entity
    {
        private const int MinutesToBookingExpires = 10;
        public bool Paid { get; private set; }
        public DateTime? BookingExpiresAt { get; private set; }
        public Guid ShowtimeId { get; private set; }
        public Showtime? Showtime { get; private set; }
        public ICollection<BookedSeat>? BookedSeats { get; private set; }

        public Ticket(Guid showtimeId) : base()
        {
            DomainExceptionValidation.When(showtimeId == Guid.Empty, "Showtime is required");
            ShowtimeId = showtimeId;
            Paid = false;
            BookingExpiresAt = DateTime.Now.AddMinutes(MinutesToBookingExpires);
        }

        public void ConfirmPayment()
        {
            DomainExceptionValidation.When(Paid, "Ticket is already paid");
            Paid = true;
            UpdatedAt = DateTime.Now;
            BookingExpiresAt = null;
        }

        public void Cancel()
        {
            DomainExceptionValidation.When(Paid, "Ticket is already paid");
            DomainExceptionValidation.When(IsDeleted, "Ticket is already canceled");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
