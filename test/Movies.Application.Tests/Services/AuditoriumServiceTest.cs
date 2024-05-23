using AutoMapper;
using MediatR;
using Moq;
using Movies.Application.DTOs;
using Movies.Application.Mediators.Auditoriums.Queries;
using Movies.Application.Services;
using Movies.Domain.Entities;

namespace Movies.Application.Tests.Services
{
    public class AuditoriumServiceTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;

        public AuditoriumServiceTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Get_WithValidData_ReturnOK()
        {
            //Arrange
            var auditoriumService = new AuditoriumService(_mediator.Object, _mapper.Object);
            var auditoriumDto = new AuditoriumDto();
            var auditorium = new Auditorium();

            _mediator.Setup(x => x.Send(It.IsAny<GetAuditoriumsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Auditorium> { auditorium });

            _mapper.Setup(x => x.Map<ICollection<AuditoriumDto>>(It.IsAny<List<Auditorium>>()))
                .Returns(new List<AuditoriumDto> { auditoriumDto });

            //Act
            var result = await auditoriumService.GetAsync(new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetAuditoriumsQuery>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnOK()
        {
            //Arrange
            var auditoriumService = new AuditoriumService(_mediator.Object, _mapper.Object);
            var auditoriumDto = new AuditoriumDto();
            var auditorium = new Auditorium();

            _mediator.Setup(x => x.Send(It.IsAny<GetAuditoriumByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditorium);

            _mapper.Setup(x => x.Map<AuditoriumDto>(It.IsAny<Auditorium>()))
                .Returns(auditoriumDto);

            //Act
            var result = await auditoriumService.GetByIdAsync(Guid.NewGuid(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetAuditoriumByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }
    }
}
