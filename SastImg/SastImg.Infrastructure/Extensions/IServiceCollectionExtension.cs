using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Primitives.Command;
using Primitives.Common.Policies;
using Primitives.DomainEvent;
using Primitives.Request;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using SastImg.Infrastructure.Event;
using SastImg.Infrastructure.Persistence;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using SastImg.Infrastructure.Persistence.TypeConverters;
using SastImg.Infrastructure.QueryCache.AlbumCaching;
using SastImg.Infrastructure.QueryCache.ImageCaching;
using SastImg.Infrastructure.QueryRepositories;
using Shared.Response.Builders;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureOptions(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }

        public static IServiceCollection ConfigureDatabase(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<SastImgDbContext>(options =>
            {
                options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });
            SqlMapper.AddTypeHandler(new UriStringConverter());
            services.AddSingleton<DbDataSource>(
                new NpgsqlDataSourceBuilder(connectionString).Build()
            );
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString)
            );

            services.AddScoped<IGetAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<IGetAlbumRepository, AlbumQueryRepository>();
            return services;
        }

        public static IServiceCollection ConfigureCache(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddResponseCaching();
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString)
            );
            services.AddScoped<ICache<IEnumerable<AlbumDto>>, GetAlbumsCache>();
            services.AddScoped<ICache<DetailedAlbumDto>, GetAlbumCache>();
            services.AddScoped<ICache<IEnumerable<ImageDto>>, GetImagesCache>();
            services.AddScoped<ICache<DetailedImageDto>, GetImageCache>();
            return services;
        }

        public static IServiceCollection ConfigureRateLimiter(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Concurrency Limiter
            services.AddRateLimiter(options =>
            {
                options.OnRejected += RateLimitOnRejected;
                options.AddConcurrencyLimiter(
                    RateLimiterPolicyNames.Concurrency,
                    options =>
                    {
                        var value = configuration
                            .GetSection(nameof(RateLimiter))
                            .GetSection(RateLimiterPolicyNames.Concurrency)
                            .Get<ConcurrencyLimiterOptions>()!;
                        options.PermitLimit = value.PermitLimit;
                        options.QueueLimit = value.QueueLimit;
                        options.QueueProcessingOrder = value.QueueProcessingOrder;
                    }
                );
            });
            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandSender, InternalEventBus>();
            services.AddScoped<IDomainEventPublisher, InternalEventBus>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            });

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Description = "Authorization Header \r\nExample:'Bearer 123456789'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                options.AddSecurityRequirement(requirement);
                var xmlFilename = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }

        /// <summary>
        /// Triggered when reaching the rate limiter's max, containing the response content.
        /// </summary>
        private static ValueTask RateLimitOnRejected(
            OnRejectedContext context,
            CancellationToken cancellationToken
        )
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter = (
                    (int)retryAfter.TotalSeconds
                ).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context
                .HttpContext
                .Response
                .WriteAsJsonAsync(Responses.TooManyRequests, cancellationToken);

            return ValueTask.CompletedTask;
        }
    }
}
