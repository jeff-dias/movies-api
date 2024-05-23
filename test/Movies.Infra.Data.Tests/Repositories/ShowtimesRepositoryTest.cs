using Movies.Domain.Entities;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class ShowtimesRepositoryTest : BaseRepositoryTest
    {
        [Fact]
        public async Task Create_WhenDataIsValid_ReturnCreatedOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            var auditorium = new Auditorium();
            var showtime = new Showtime(DateTime.Now.AddDays(1), auditorium.Id, "external-movie-id");

            //Act & Assert
            var repository = new ShowtimesRepository(context);

            var result = await repository.CreateAsync(showtime, CancellationToken.None);
            Assert.Equal(result, showtime);
        }

        [Fact]
        public async Task GetAll_WhenDataExists_ReturnOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            var auditorium = new Auditorium();
            var showtime = new Showtime(DateTime.Now.AddDays(1), auditorium.Id, "external-movie-id");
            context.Add(auditorium);
            context.Add(showtime);
            context.SaveChanges();

            //Act & Assert
            var repository = new ShowtimesRepository(context);

            var result = await repository.GetAllAsync(x => true, CancellationToken.None);
            Assert.Contains(showtime, result);
        }

        [Fact]
        public async Task GetEnrichedById_WhenDataExists_ReturnOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            var auditorium = new Auditorium();
            var showtime = new Showtime(DateTime.Now.AddDays(1), auditorium.Id, "external-movie-id");
            context.Add(auditorium);
            context.Add(showtime);
            context.SaveChanges();

            //Act & Assert
            var repository = new ShowtimesRepository(context);

            var result = await repository.GetEnrichedByIdAsync(showtime.Id, CancellationToken.None);
            Assert.Equal(showtime, result);
        }

        [Fact]
        public async Task GetEnrichedById_WhenDataDoesNotExist_ReturnApplicationException()
        {
            //Arrange
            using var context = new MoviesContext(_options);

            //Act & Assert
            var repository = new ShowtimesRepository(context);

            await Assert.ThrowsAsync<ApplicationException>(() => repository.GetEnrichedByIdAsync(Guid.NewGuid(), CancellationToken.None));
        }
    }
}
