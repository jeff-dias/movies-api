namespace Movies.Domain.Tests.Entities
{
    public class AuditoriumTest
    {
        [Fact]
        [Trait("Domain", "Auditorium")]
        public void Create_WithValidParam_ResultOK()
        {
            Action action = () => new Auditorium();
            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        [Trait("Domain", "Auditorium")]
        public void Update_WithValidParam_ResultOK()
        {
            Action action = () => new Auditorium().Update();
            action.Should().NotThrow<DomainExceptionValidation>();
        }
    }
}
