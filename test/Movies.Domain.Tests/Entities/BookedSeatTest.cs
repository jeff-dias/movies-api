namespace Movies.Domain.Tests.Entities
{
    public class BookedSeatTest
    {
        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void Create_WithValidParam_ResultOK()
        {
            Action action = () => new BookedSeat(Guid.NewGuid());
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void Create_WithInvalidSeatId_ResultDomainException()
        {
            Action action = () => new BookedSeat(Guid.Empty);
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Seat is required");
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void ConfirmBooking_WithValidTicketId_ResultOK()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            Action action = () => bookedSeat.ConfirmBooking(Guid.NewGuid());
            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void ConfirmBooking_WithInvalidTicketId_ResultDomainException()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            Action action = () => bookedSeat.ConfirmBooking(Guid.Empty);
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Ticket is required");
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void ConfirmBooking_Duplicated_ResultDomainException()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            bookedSeat.ConfirmBooking(Guid.NewGuid());

            Action action = () => bookedSeat.ConfirmBooking(Guid.NewGuid());
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Seat is already booked");
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void CancelBooking_WithValidParams_ResultOK()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            bookedSeat.ConfirmBooking(Guid.NewGuid());
            Action action = () => bookedSeat.CancelBooking();
            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void CancelBooking_WhenTicketIdIsInvalid_ResultDomainException()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            Action action = () => bookedSeat.CancelBooking();
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Seat is not booked");
        }

        [Fact]
        [Trait("Domain", "BookedSeat")]
        public void CancelBooking_Duplicated_ResultDomainException()
        {
            var bookedSeat = new BookedSeat(Guid.NewGuid());
            bookedSeat.ConfirmBooking(Guid.NewGuid());
            bookedSeat.CancelBooking();
            Action action = () => bookedSeat.CancelBooking();
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Seat booking is already canceled");
        }
    }
}
