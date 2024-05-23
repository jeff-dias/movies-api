using Moq;
using Movies.Application.Mediators.Tickets.Commands;
using Movies.Application.Mediators.Tickets.Handlers;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Tickets
{
    public class TicketConfirmPaymentCommandHandlerTest
    {
        private readonly Mock<ITicketsRepository> _repository;

        public TicketConfirmPaymentCommandHandlerTest()
        {
            _repository = new Mock<ITicketsRepository>();
        }

        [Fact]
        public async void TicketConfirmPaymentCommand_WhenParamsAreValid_ReturnEntityOK()
        {
            //Arrange
            var ticketCommand = new TicketConfirmPaymentCommand(Guid.NewGuid());
            var ticket = new Ticket(Guid.NewGuid());

            _repository.Setup(x => x.ConfirmPaymentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            var handler = new TicketConfirmPaymentCommandHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(ticketCommand, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
