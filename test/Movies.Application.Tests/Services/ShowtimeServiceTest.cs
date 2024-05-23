using AutoMapper;
using MediatR;
using Moq;
using Movies.Application.DTOs;
using Movies.Application.Interfaces;
using Movies.Application.Mediators.Showtimes.Commands;
using Movies.Application.Mediators.Showtimes.Queries;
using Movies.Application.Services;
using Movies.Domain.Entities;

namespace Movies.Application.Tests.Services
{
    public class ShowtimeServiceTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IExternalMoviesService> _externalMoviesService;

        public ShowtimeServiceTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            _externalMoviesService = new Mock<IExternalMoviesService>();
        }

        [Fact]
        public async Task Create_WithValidData_ReturnOK()
        {
            //Arrange
            var showtimeService = new ShowtimeService(_mediator.Object, _mapper.Object, _externalMoviesService.Object);
            var showtimeDto = new ShowtimeDto();
            var showtime = new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-movie-id");

            _mediator.Setup(x => x.Send(It.IsAny<ShowtimeCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(showtime);

            _mapper.Setup(x => x.Map<ShowtimeDto>(It.IsAny<Showtime>()))
                .Returns(showtimeDto);

            //Act
            var result = await showtimeService.CreateAsync(new ShowtimeDto(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<ShowtimeCreateCommand>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_WithValidData_ReturnOK()
        {
            //Arrange
            var showtimeDto = new ShowtimeDto
            {
                ExternalMovieId = "external-movie-id"
            };
            var showtime = new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-movie-id");
            var showtimes = new List<Showtime> { showtime };

            var externalMovieDto = new ExternalMovieDto
            {
                Id = "external-movie-id"
            };

            _externalMoviesService.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ExternalMovieDto> { externalMovieDto });

            _mediator.Setup(x => x.Send(It.IsAny<GetShowtimesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(showtimes);

            _mapper.Setup(x => x.Map<ICollection<ShowtimeDto>>(It.IsAny<ICollection<Showtime>>()))
                .Returns(new List<ShowtimeDto> { showtimeDto });

            var showtimeService = new ShowtimeService(_mediator.Object, _mapper.Object, _externalMoviesService.Object);

            //Act
            var result = await showtimeService.GetAsync(new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetShowtimesQuery>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEnrichedById_WithValidId_ReturnOK()
        {
            //Arrange
            var showtimeService = new ShowtimeService(_mediator.Object, _mapper.Object, _externalMoviesService.Object);
            var showtimeDto = new ShowtimeDto();
            var showtime = new Showtime(DateTime.Now.AddDays(1), Guid.NewGuid(), "external-movie-id");

            _externalMoviesService.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ExternalMovieDto());

            _mediator.Setup(x => x.Send(It.IsAny<GetShowtimeEnrichedByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(showtime);

            _mapper.Setup(x => x.Map<ShowtimeDto>(It.IsAny<Showtime>()))
                .Returns(showtimeDto);

            //Act
            var result = await showtimeService.GetEnrichedByIdAsync(Guid.NewGuid(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetShowtimeEnrichedByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

    }
}
