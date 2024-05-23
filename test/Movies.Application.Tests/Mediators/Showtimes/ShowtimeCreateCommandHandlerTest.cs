using Moq;
using Movies.Application.Mediators.Showtimes.Commands;
using Movies.Application.Mediators.Showtimes.Handlers;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Showtimes
{
    public class ShowtimeCreateCommandHandlerTest
    {
        private readonly Mock<IShowtimesRepository> _repository;

        public ShowtimeCreateCommandHandlerTest()
        {
            _repository = new Mock<IShowtimesRepository>();
        }

        [Fact]
        public async void ShowtimeCreateCommand_WhenParamsAreValid_ReturnEntityOK()
        {
            //Arrange
            var showtimeCommand = new ShowtimeCreateCommand
            {
                SessionDate = DateTime.Now.AddDays(1),
                AuditoriumId = Guid.NewGuid(),
                ExternalMovieId = "external-movie-id"
            };
            var showtime = new Showtime(showtimeCommand.SessionDate, showtimeCommand.AuditoriumId, showtimeCommand.ExternalMovieId);
            _repository.Setup(x => x.CreateAsync(It.IsAny<Showtime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(showtime);

            var handler = new ShowtimeCreateCommandHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(showtimeCommand, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
