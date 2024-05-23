using Moq;
using Movies.Application.Mediators.Auditoriums.Handlers;
using Movies.Application.Mediators.Auditoriums.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Auditoriums
{
    public class GetAuditoriumByIdQueryHandlerTest
    {
        private readonly Mock<IAuditoriumsRepository> _repository;

        public GetAuditoriumByIdQueryHandlerTest()
        {
            _repository = new Mock<IAuditoriumsRepository>();
        }

        [Fact]
        public async void GetAuditoriumByIdQuery_WhenDataIsOnDb_ReturnEntityOK()
        {
            //Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Auditorium());

            var query = new GetAuditoriumByIdQuery(Guid.NewGuid());
            var handler = new GetAuditoriumByIdQueryHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
