using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Movies.Application.DTOs;
using Movies.Application.Extensions;
using Movies.Application.Interfaces;

namespace Movies.Application.Services
{
    public class ExternalMoviesService : IExternalMoviesService
    {
        private const string CacheKey = "external-movies";

        private readonly Movies.MoviesClient _moviesClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ExternalMoviesService> _logger;
        private readonly IMapper _mapper;

        public ExternalMoviesService(Movies.MoviesClient moviesClient,
        IDistributedCache cache,
        ILogger<ExternalMoviesService> logger,
        IMapper mapper)
        {
            _moviesClient = moviesClient;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ICollection<ExternalMovieDto>> GetAsync(CancellationToken cancel)
        {
            var request = new Empty();
            try
            {
                var response = await _moviesClient.GetAllAsync(request, cancellationToken: cancel);

                if (response != null)
                {
                    var mappedResult = _mapper.Map<ICollection<ExternalMovieDto>>(response.Movies);
                    await _cache.WarmAsync(CacheKey, mappedResult, cancel);
                    return mappedResult;
                }

                return null;
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    _logger.LogError(ex.Message);

                    _logger.LogInformation($"Using cached response for the key: {CacheKey}");
                    _cache.TryGetValue<ICollection<ExternalMovieDto>>(CacheKey, out var cachedResponse);

                    if (cachedResponse != null)
                    {
                        return cachedResponse;
                    }

                    return await GetAsync(cancel);
                }

                throw;
            }
        }

        public async Task<ExternalMovieDto> GetByIdAsync(string id, CancellationToken cancel)
        {
            var cacheKey = $"{CacheKey}-{id}";

            var request = new GetByIdRequest { Id = id };
            try
            {
                var response = await _moviesClient.GetByIdAsync(request, cancellationToken: cancel);

                if (response != null)
                {
                    var mappedResult = _mapper.Map<ExternalMovieDto>(response);
                    await _cache.WarmAsync(cacheKey, mappedResult, cancel);
                    return mappedResult;
                }

                return null;
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    _logger.LogInformation($"Using cached response for the key: {cacheKey}");
                    _cache.TryGetValue<ExternalMovieDto>(cacheKey, out var cachedResponse);

                    if (cachedResponse != null)
                    {
                        return cachedResponse;
                    }

                    return await GetByIdAsync(id, cancel);
                }

                throw;
            }
        }
    }
}
