namespace Movies.Domain.Tests.Entities
{
    public class ShowtimeTest
    {
        [Fact]
        [Trait("Domain", "Showtime")]
        public void Create_WithValidParam_ResultOK()
        {
            Action action = () => new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-id-1");
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        [Trait("Domain", "Showtime")]
        public void Create_WithInvalidSessionDate_ResultDomainException()
        {
            Action action = () => new Showtime(DateTime.Now.AddDays(-1), Guid.NewGuid(), "external-id-1");
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Date is old, you can create an event only for future");
        }

        [Fact]
        [Trait("Domain", "Showtime")]
        public void Create_WithInvalidAuditoriumId_ResultDomainException()
        {
            Action action = () => new Showtime(DateTime.Now.AddDays(1), Guid.Empty, "external-id-1");
            action.Should().Throw<DomainExceptionValidation>().WithMessage("Auditorium is required");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        [Trait("Domain", "Showtime")]
        public void Create_WithInvalidExternalMovieId_ResultDomainException(string externalId)
        {
            Action action = () => new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), externalId);
            action.Should().Throw<DomainExceptionValidation>().WithMessage("External movie id is required");
        }
    }
}
