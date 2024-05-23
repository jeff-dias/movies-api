using Moq;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Tickets.Handlers;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Tickets
{
    public class TicketCancelCommandHandlerTest
    {
        private readonly Mock<ITicketsRepository> _repository;

        public TicketCancelCommandHandlerTest()
        {
            _repository = new Mock<ITicketsRepository>();
        }

        [Fact]
        public async void TicketCancelCommand_WhenParamsAreValid_ReturnEntityOK()
        {
            //Arrange
            var ticketCommand = new TicketCancelCommand(Guid.NewGuid());
            var ticket = new Ticket(Guid.NewGuid());

            _repository.Setup(x => x.CancelAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            var handler = new TicketCancelCommandHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(ticketCommand, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
