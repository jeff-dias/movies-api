using Moq;
using Movies.Application.Mediators.Tickets.Handlers;
using Movies.Application.Mediators.Tickets.Queries;
using Movies.Domain.Entities;
using Movies.Domain.Interfaces;

namespace Movies.Application.Tests.Mediators.Tickets
{
    public class GetTicketEnrichedByIdQueryHandlerTest
    {
        private readonly Mock<ITicketsRepository> _repository;

        public GetTicketEnrichedByIdQueryHandlerTest()
        {
            _repository = new Mock<ITicketsRepository>();
        }

        [Fact]
        public async void GetTicketEnrichedByIdQuery_WhenDataIsOnDb_ReturnEntityOK()
        {
            //Arrange
            _repository.Setup(x => x.GetEnrichedByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Ticket(Guid.NewGuid()));

            var query = new GetTicketEnrichedByIdQuery(Guid.NewGuid());
            var handler = new GetTicketEnrichedByIdQueryHandler(_repository.Object);

            //Act
            var entity = await handler.Handle(query, new CancellationToken());

            //Assert
            Assert.NotNull(entity);
        }
    }
}
