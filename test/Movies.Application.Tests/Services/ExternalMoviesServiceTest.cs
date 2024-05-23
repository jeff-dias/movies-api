using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Testing;
using Moq;
using Movies.Application.DTOs;
using Movies.Application.Services;

namespace Movies.Application.Tests.Services
{
    public class ExternalMoviesServiceTest
    {
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<ExternalMoviesService>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly MoviesResponse _expectedClientResult;
        private string ExpectedListCacheResult;

        private string ExpectedEntityCacheResult;

        private void ArrangeMapperAndMocks()
        {
            var movie1 = new MovieResponse
            {
                Id = "tt0000001",
                Rank = "1",
                Title = "Carmencita",
                FullTitle = "Carmencita (1894)",
                Year = "1894",
                Image = "image.jpg",
                Crew = "William K.L. Dickson (dir.)",
                ImdbRating = "5.9",
                ImdbRatingCount = "1032"
            };
            var movie2 = new MovieResponse
            {
                Id = "tt0000002",
                Rank = "2",
                Title = "Carmencita",
                FullTitle = "Carmencita (1894)",
                Year = "1894",
                Image = "image.jpg",
                Crew = "William K.L. Dickson (dir.)",
                ImdbRating = "5.9",
                ImdbRatingCount = "1032"
            };
            _expectedClientResult.Movies.Add(movie1);
            _expectedClientResult.Movies.Add(movie2);

            var movieDto1 = new ExternalMovieDto
            {
                Id = "tt0000002",
                Rank = "2",
                Title = "Carmencita",
                FullTitle = "Carmencita (1894)",
                Year = "1894",
                Image = "image.jpg",
                Crew = "William K.L. Dickson (dir.)",
                ImdbRating = "5.9",
                ImdbRatingCount = "1032"
            };
            var movieDto2 = new ExternalMovieDto
            {
                Id = "tt0000002",
                Rank = "2",
                Title = "Carmencita",
                FullTitle = "Carmencita (1894)",
                Year = "1894",
                Image = "image.jpg",
                Crew = "William K.L. Dickson (dir.)",
                ImdbRating = "5.9",
                ImdbRatingCount = "1032"
            };
            ExpectedListCacheResult = JsonSerializer.Serialize(new List<ExternalMovieDto> { movieDto1, movieDto2 });
            ExpectedEntityCacheResult = JsonSerializer.Serialize(movieDto1);

            _mapper.Setup(x => x.Map<ICollection<ExternalMovieDto>>(It.IsAny<RepeatedField<MovieResponse>>()))
                .Returns(new List<ExternalMovieDto> { movieDto1, movieDto2 });
            _mapper.Setup(x => x.Map<ExternalMovieDto>(It.IsAny<MovieResponse>()))
                .Returns(movieDto1);
        }

        private Movies.MoviesClient GetMockRpcClient(bool timeout = false)
        {
            var mockClient = new Mock<Movies.MoviesClient>();

            if (timeout)
            {
                mockClient
                    .Setup(m => m.GetAllAsync(
                        It.IsAny<Empty>(), null, null, CancellationToken.None))
                    .Throws(new RpcException(new Status(StatusCode.DeadlineExceeded, "Deadline Exceeded")));
                mockClient
                    .Setup(m => m.GetByIdAsync(
                        It.IsAny<GetByIdRequest>(), null, null, CancellationToken.None))
                    .Throws(new RpcException(new Status(StatusCode.DeadlineExceeded, "Deadline Exceeded")));
            }
            else
            {
                var fakeListCall = TestCalls.AsyncUnaryCall(
                    Task.FromResult(_expectedClientResult),
                    Task.FromResult(new Metadata()),
                    () => Status.DefaultSuccess,
                    () => new Metadata(),
                    () => { });

                mockClient
                    .Setup(m => m.GetAllAsync(It.IsAny<Empty>(), null, null, CancellationToken.None))
                    .Returns(fakeListCall);

                var fakeByIdCall = TestCalls.AsyncUnaryCall(
                                    Task.FromResult(_expectedClientResult.Movies.First()),
                                    Task.FromResult(new Metadata()),
                                    () => Status.DefaultSuccess,
                                    () => new Metadata(),
                                    () => { });
                mockClient
                    .Setup(m => m.GetByIdAsync(It.IsAny<GetByIdRequest>(), null, null, CancellationToken.None))
                    .Returns(fakeByIdCall);
            }

            return mockClient.Object;
        }

        public ExternalMoviesServiceTest()
        {
            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<ExternalMoviesService>>();
            _mapper = new Mock<IMapper>();
            _expectedClientResult = new MoviesResponse();
        }

        [Fact]
        public async Task Get_WhenProviderApiIsAvailable_ReturnOK()
        {
            //Arrange
            ArrangeMapperAndMocks();
            var rpcClient = GetMockRpcClient();

            var service = new ExternalMoviesService(rpcClient, _cache.Object, _logger.Object, _mapper.Object);

            //Act
            var entity = await service.GetAsync(new CancellationToken());

            //Assert
            _cache.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
            _cache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Get_WhenProviderApiIsNotAvailable_ReturnOKFromCache()
        {
            //Arrange
            ArrangeMapperAndMocks();
            var rpcClient = GetMockRpcClient(timeout: true);
            _cache.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(Encoding.UTF8.GetBytes(ExpectedListCacheResult));

            var service = new ExternalMoviesService(rpcClient, _cache.Object, _logger.Object, _mapper.Object);

            //Act
            var entity = await service.GetAsync(new CancellationToken());

            //Assert
            _cache.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
            _cache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task GetById_WhenProviderApiIsAvailable_ReturnOK()
        {
            //Arrange
            ArrangeMapperAndMocks();
            var rpcClient = GetMockRpcClient();

            var service = new ExternalMoviesService(rpcClient, _cache.Object, _logger.Object, _mapper.Object);

            //Act
            var entity = await service.GetByIdAsync("external-movie-id", new CancellationToken());

            //Assert
            _cache.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
            _cache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task GetById_WhenProviderApiIsNotAvailable_ReturnOKFromCache()
        {
            //Arrange
            ArrangeMapperAndMocks();
            var rpcClient = GetMockRpcClient(timeout: true);
            _cache.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(Encoding.UTF8.GetBytes(ExpectedEntityCacheResult));

            var service = new ExternalMoviesService(rpcClient, _cache.Object, _logger.Object, _mapper.Object);

            //Act
            var entity = await service.GetByIdAsync("external-movie-id", new CancellationToken());

            //Assert
            _cache.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
            _cache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.NotNull(entity);
        }

    }
}
