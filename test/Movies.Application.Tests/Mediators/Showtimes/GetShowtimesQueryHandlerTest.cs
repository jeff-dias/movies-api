using Moq;
using Movies.Application.Mediators.Showtimes.Handlers;
using Movies.Application.Mediators.Showtimes.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Showtimes
{
    public class GetShowtimesQueryHandlerTest
    {
        private readonly Mock<IShowtimesRepository> _repository;

        public GetShowtimesQueryHandlerTest()
        {
            _repository = new Mock<IShowtimesRepository>();
        }

        [Fact]
        public async void GetShowtimesQuery_WhenDataIsOnDb_ReturnEntityOK()
        {
            //Arrange
            var showtimes = new List<Showtime> { new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-movie-id") };

            _repository.Setup(x => x.GetAllAsync(x => true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(showtimes);

            var query = new GetShowtimesQuery();
            var handler = new GetShowtimesQueryHandler(_repository.Object);

            //Act
            var entities = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.Equal(1, entities.Count);
        }
    }
}
