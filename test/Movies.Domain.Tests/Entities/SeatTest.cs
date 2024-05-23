namespace Movies.Domain.Tests.Entities
{
    public class SeatTest
    {
        [Fact]
        [Trait("Domain", "Seat")]
        public void Create_WithValidParam_ResultOK()
        {
            Action action = () => new Seat(1, 1, Guid.NewGuid());
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "Seat")]
        public void Create_WithInvalidSeatRow_ResultDomainException()
        {
            Action action = () => new Seat(0, 1, Guid.NewGuid());
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Row must be greater than 0");
        }

        [Fact]
        [Trait("Domain", "Seat")]
        public void Create_WithInvalidSeatNumber_ResultDomainException()
        {
            Action action = () => new Seat(1, 0, Guid.NewGuid());
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Seat number must be greater than 0");
        }

        [Fact]
        [Trait("Domain", "Seat")]
        public void Create_WithInvalidAuditoriumId_ResultDomainException()
        {
            Action action = () => new Seat(1, 1, Guid.Empty);
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Auditorium is required");
        }
    }
}
