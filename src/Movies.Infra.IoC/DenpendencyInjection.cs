using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Interfaces;
using Movies.Application.Mappings;
using Movies.Application.Services;
using Movies.Domain.Interfaces;
using Movies.Infra.Data.Context;
using Movies.Infra.Data.Repositories;

namespace Movies.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MoviesContext>(options =>
            {
                options.UseInMemoryDatabase("MoviesDb")
                    .EnableSensitiveDataLogging()
                    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["REDIS_CACHE_URL"];
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { configuration["REDIS_CACHE_URL"] }
                };
            });

            services.AddScoped<IAuditoriumsRepository, AuditoriumsRepository>();
            services.AddScoped<IBookedSeatsRepository, BookedSeatsRepository>();
            services.AddScoped<ISeatsRepository, SeatsRepository>();
            services.AddScoped<IShowtimesRepository, ShowtimesRepository>();
            services.AddScoped<ITicketsRepository, TicketsRepository>();

            services.AddScoped<IAuditoriumService, AuditoriumService>();
            services.AddScoped<IExternalMoviesService, ExternalMoviesService>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<ITicketService, TicketService>();

            services.AddGrpcClient<Application.Movies.MoviesClient>(client =>
            {
                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                client.Address = new Uri(configuration["PROVIDER_API_URL"] ?? "");
                client.ChannelOptionsActions.Add(o =>
                {
                    o.Credentials = ChannelCredentials.Insecure;
                    o.HttpHandler = httpHandler;
                });
                client.CallOptionsActions.Add(o =>
                {
                    o.CallOptions = new CallOptions().WithDeadline(DateTime.UtcNow.AddSeconds(1));
                });
            });

            services.AddAutoMapper(typeof(DomainToDtoMappingProfile));
            services.AddAutoMapper(typeof(DtoToCommandMappingProfile));

            var myHandlers = AppDomain.CurrentDomain.Load("Movies.Application");
            services.AddMediatR(c => c.RegisterServicesFromAssembly(myHandlers));

            Seed.SampleData.Seed(services);

            return services;
        }
    }
}
