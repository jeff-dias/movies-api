using Microsoft.EntityFrameworkCore;
using Movies.Infra.Data.Context;

namespace Movies.Infra.Data.Tests.Repositories
{
    public class BaseRepositoryTest : IDisposable
    {
        protected readonly DbContextOptions<MoviesContext> _options;

        public BaseRepositoryTest()
        {
            var databaseName = Guid.NewGuid().ToString();
            _options = new DbContextOptionsBuilder<MoviesContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;
        }

        public void Dispose()
        {
            using var context = new MoviesContext(_options);
            context.Database.EnsureDeleted();
        }
    }
}
