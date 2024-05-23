namespace Movies.Domain.Tests.Entities
{
    public class TicketTest
    {
        [Fact]
        [Trait("Domain", "Ticket")]
        public void Create_WithValidParam_ResultOK()
        {
            Action action = () => new Ticket(Guid.NewGuid());
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "Ticket")]
        public void Create_WithInvalidShowtimeId_ResultDomainException()
        {
            Action action = () => new Ticket(Guid.Empty);
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Showtime is required");
        }

        [Fact]
        [Trait("Domain", "Ticket")]
        public void ConfirmPayment_WhenIsNotPaid_ResultOK()
        {
            var ticket = new Ticket(Guid.NewGuid());

            Action action = () => ticket.ConfirmPayment();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "Ticket")]
        public void ConfirmPayment_WhenIsAlreadyPaid_ResultDomainException()
        {
            var ticket = new Ticket(Guid.NewGuid());
            ticket.ConfirmPayment();

            Action action = () => ticket.ConfirmPayment();
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Ticket is already paid");
        }

        [Fact]
        [Trait("Domain", "Ticket")]
        public void Cancel_WhenIsNotCanceled_ResultOK()
        {
            var ticket = new Ticket(Guid.NewGuid());

            Action action = () => ticket.Cancel();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "Ticket")]
        public void Cancel_WhenIsAlreadyCanceled_ResultDomainException()
        {
            var ticket = new Ticket(Guid.NewGuid());
            ticket.Cancel();

            Action action = () => ticket.Cancel();
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Ticket is already canceled");
        }
    }
}
