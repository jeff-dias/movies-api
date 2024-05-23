namespace Movies.Domain.Tests.Entities
{
    public class EntityTest
    {
        private class TestEntity : Entity
        {
            public TestEntity() : base()
            {
            }
        }

        [Fact]
        [Trait("Domain", "Entity")]
        public void CreateEntity_ResultObjectValidState()
        {
            Action action = () => new TestEntity();
            action.Should().NotThrow<DomainExceptionValidation>();
        }
    }
}
