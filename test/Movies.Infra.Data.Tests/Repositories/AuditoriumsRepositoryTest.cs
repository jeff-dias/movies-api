using Movies.Domain.Entities;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class AuditoriumsRepositoryTest : BaseRepositoryTest
    {
        [Fact]
        public async Task Get_WhenDbIsEmpty_ReturnEmpty()
        {
            //Arrange
            using var context = new MoviesContext(_options);

            //Act
            var auditoriumsRepository = new AuditoriumsRepository(context);
            var result = await auditoriumsRepository.GetAsync(CancellationToken.None);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_WhenDbHasData_ReturnDataOK()
        {
            //Arrange
            using var context = new MoviesContext(_options);
            context.Auditoriums.Add(new Auditorium());
            context.Auditoriums.Add(new Auditorium());
            context.Auditoriums.Add(new Auditorium());
            context.SaveChanges();

            //Act
            var auditoriumsRepository = new AuditoriumsRepository(context);
            var result = await auditoriumsRepository.GetAsync(CancellationToken.None);

            //Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task Get_WhenDbHasThreeItemsButOneIsDeleted_ReturnOnlyNotDeletedOnes()
        {
            //Arrange
            var deletedOne = new Auditorium();
            deletedOne.Delete();
            using var context = new MoviesContext(_options);
            context.Auditoriums.Add(new Auditorium());
            context.Auditoriums.Add(deletedOne);
            context.Auditoriums.Add(new Auditorium());
            context.SaveChanges();

            //Act
            var auditoriumsRepository = new AuditoriumsRepository(context);
            var result = await auditoriumsRepository.GetAsync(CancellationToken.None);

            //Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetById_WhenDbIsEmpty_ReturnApplicationException()
        {
            //Arrange
            using var context = new MoviesContext(_options);

            //Act & Assert
            var auditoriumsRepository = new AuditoriumsRepository(context);
            await Assert.ThrowsAsync<ApplicationException>(() => auditoriumsRepository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None));
        }

        [Fact]
        public async Task GetById_WhenDbHasData_ReturnDataOK()
        {
            //Arrange
            var auditorium = new Auditorium();

            using var context = new MoviesContext(_options);
            context.Auditoriums.Add(auditorium);
            context.SaveChanges();

            //Act
            var auditoriumsRepository = new AuditoriumsRepository(context);
            var result = await auditoriumsRepository.GetByIdAsync(auditorium.Id, CancellationToken.None);

            //Assert
            Assert.Equal(auditorium, result);
        }

        [Fact]
        public async Task GetById_WhenTheItemIsDeleted_ReturnApplicationException()
        {
            //Arrange
            var auditorium = new Auditorium();
            auditorium.Delete();

            using var context = new MoviesContext(_options);
            context.Auditoriums.Add(auditorium);
            context.SaveChanges();

            //Act & Assert
            var auditoriumsRepository = new AuditoriumsRepository(context);
            await Assert.ThrowsAsync<ApplicationException>(() => auditoriumsRepository.GetByIdAsync(auditorium.Id, CancellationToken.None));
        }
    }
}
