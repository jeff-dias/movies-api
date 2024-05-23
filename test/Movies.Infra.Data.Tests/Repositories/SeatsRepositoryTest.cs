using Movies.Domain.Entities;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class SeatsRepositoryTest : BaseRepositoryTest
    {
        [Fact]
        public async Task GetByIdsAsync_WhenDbHasData_ReturnDataOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            var auditorium = new Auditorium();
            var seat1 = new Seat(1, 1, auditorium.Id);
            var seat2 = new Seat(1, 2, auditorium.Id);
            var seat3 = new Seat(1, 3, auditorium.Id);
            var seats = new List<Seat>
            {
                seat1,
                seat2,
                seat3
            };
            context.Auditoriums.Add(auditorium);
            context.Seats.AddRange(seats);
            context.SaveChanges();

            //Act & Assert
            var repository = new SeatsRepository(context);

            var result = await repository.GetByIdsAsync(seats.Select(x => x.Id).ToList(), CancellationToken.None);
            Assert.Contains(seat1, result);
            Assert.Contains(seat2, result);
            Assert.Contains(seat3, result);
        }

        [Fact]
        public async Task GetByIdsAsync_WhenDbIsEmpty_ReturnEmpty()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            var auditorium = new Auditorium();
            var seat1 = new Seat(1, 1, auditorium.Id);
            var seat2 = new Seat(1, 2, auditorium.Id);
            var seat3 = new Seat(1, 3, auditorium.Id);
            var seats = new List<Seat>
            {
                seat1,
                seat2,
                seat3
            };

            //Act & Assert
            var repository = new SeatsRepository(context);

            var result = await repository.GetByIdsAsync(seats.Select(x => x.Id).ToList(), CancellationToken.None);
            Assert.Empty(result);
        }
    }
}
