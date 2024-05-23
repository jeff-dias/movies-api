using Moq;
using Movies.Application.Mediators.Auditoriums.Handlers;
using Movies.Application.Mediators.Auditoriums.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Auditoriums
{
    public class GetAuditoriumsQueryHandlerTest
    {
        private readonly Mock<IAuditoriumsRepository> _repository;

        public GetAuditoriumsQueryHandlerTest()
        {
            _repository = new Mock<IAuditoriumsRepository>();
        }

        [Fact]
        public async void GetAuditoriumsQuery_WhenDataIsOnDb_ReturnEntityOK()
        {
            //Arrange
            var auditoriums = new List<Auditorium> { new Auditorium() };

            _repository.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditoriums);

            var query = new GetAuditoriumsQuery();
            var handler = new GetAuditoriumsQueryHandler(_repository.Object);

            //Act
            var entities = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.Equal(1, entities.Count);
        }
    }
}
