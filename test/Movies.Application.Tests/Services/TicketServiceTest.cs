using AutoMapper;
using MediatR;
using Moq;
using Movies.Application.DTOs;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Tickets.Queries;
using Movies.Application.Services;
using Movies.Domain.Entities;

namespace Movies.Application.Tests.Services
{
    public class TicketServiceTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;

        public TicketServiceTest()
        {
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Cancel_WithValidId_ReturnOK()
        {
            //Arrange
            var ticketService = new TicketService(_mediator.Object, _mapper.Object);
            var ticketDto = new TicketDto();
            var ticket = new Ticket(Guid.NewGuid());

            _mediator.Setup(x => x.Send(It.IsAny<TicketCancelCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            _mapper.Setup(x => x.Map<TicketDto>(It.IsAny<Ticket>()))
                .Returns(ticketDto);

            //Act
            var result = await ticketService.CancelAsync(Guid.NewGuid(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<TicketCancelCommand>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ConfirmPayment_WithValidId_ReturnOK()
        {
            //Arrange
            var ticketService = new TicketService(_mediator.Object, _mapper.Object);
            var ticketDto = new TicketDto();
            var ticket = new Ticket(Guid.NewGuid());

            _mediator.Setup(x => x.Send(It.IsAny<TicketConfirmPaymentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            _mapper.Setup(x => x.Map<TicketDto>(It.IsAny<Ticket>()))
                .Returns(ticketDto);

            //Act
            var result = await ticketService.ConfirmPaymentAsync(Guid.NewGuid(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<TicketConfirmPaymentCommand>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEnrichedById_WithValidId_ReturnOK()
        {
            //Arrange
            var ticketService = new TicketService(_mediator.Object, _mapper.Object);
            var ticketDto = new TicketDto();
            var ticket = new Ticket(Guid.NewGuid());

            _mediator.Setup(x => x.Send(It.IsAny<GetTicketEnrichedByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            _mapper.Setup(x => x.Map<TicketDto>(It.IsAny<Ticket>()))
                .Returns(ticketDto);

            //Act
            var result = await ticketService.GetEnrichedByIdAsync(Guid.NewGuid(), new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetTicketEnrichedByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_WithValidData_ReturnOK()
        {
            //Arrange
            var ticketService = new TicketService(_mediator.Object, _mapper.Object);
            var ticketDto = new TicketDto();
            var ticket = new Ticket(Guid.NewGuid());

            _mediator.Setup(x => x.Send(It.IsAny<TicketCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            _mapper.Setup(x => x.Map<TicketDto>(It.IsAny<Ticket>()))
                .Returns(ticketDto);

            //Act
            var result = await ticketService.CreateAsync(ticketDto, new CancellationToken());

            //Assert
            _mediator.Verify(x => x.Send(It.IsAny<TicketCreateCommand>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.NotNull(result);
        }

    }
}
