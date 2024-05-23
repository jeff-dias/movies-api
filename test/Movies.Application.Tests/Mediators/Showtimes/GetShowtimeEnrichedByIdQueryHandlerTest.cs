using Moq;
using Movies.Application.Mediators.Showtimes.Handlers;
using Movies.Application.Mediators.Showtimes.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Showtimes
{
    public class GetShowtimeEnrichedByIdQueryHandlerTest
    {
        private readonly Mock<IShowtimesRepository> _repository;

        public GetShowtimeEnrichedByIdQueryHandlerTest()
        {
            _repository = new Mock<IShowtimesRepository>();
        }

        [Fact]
        public async void GetShowtimeEnrichedByIdQuery_WhenDataIsOnDb_ReturnEntityOK()
        {
            //Arrange
            _repository.Setup(x => x.GetEnrichedByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-movie-id"));

            var query = new GetShowtimeEnrichedByIdQuery(Guid.NewGuid());
            var handler = new GetShowtimeEnrichedByIdQueryHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
