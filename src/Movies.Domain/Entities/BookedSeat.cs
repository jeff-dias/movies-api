using Movies.Domain.Validation;

namespace Movies.Domain.Entities
{
    public sealed class BookedSeat : Entity
    {
        public Guid TicketId { get; private set; }
        public Ticket? Ticket { get; set; }
        public Guid SeatId { get; private set; }
        public Seat? Seat { get; private set; }

        public BookedSeat(Guid seatId) : base()
        {
            DomainExceptionValidation.When(seatId == Guid.Empty, "Seat is required");
            SeatId = seatId;
        }

        public void ConfirmBooking(Guid ticketId)
        {
            DomainExceptionValidation.When(TicketId != Guid.Empty, "Seat is already booked");
            DomainExceptionValidation.When(ticketId == Guid.Empty, "Ticket is required");
            TicketId = ticketId;
        }

        public void CancelBooking()
        {
            DomainExceptionValidation.When(TicketId == Guid.Empty, "Seat is not booked");
            DomainExceptionValidation.When(IsDeleted, "Seat booking is already canceled");
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
