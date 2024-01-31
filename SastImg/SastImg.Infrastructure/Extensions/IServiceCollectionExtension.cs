using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Request;
using Response.ExceptionHandlers;
using Response.Extensions;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Application.CategoryServices;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Application.SeedWorks;
using SastImg.Application.TagServices;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate;
using SastImg.Infrastructure.Domain;
using SastImg.Infrastructure.Domain.AlbumEntity;
using SastImg.Infrastructure.Domain.AlbumEntity.Caching;
using SastImg.Infrastructure.Domain.CategoryEntity;
using SastImg.Infrastructure.Domain.ImageEntity;
using SastImg.Infrastructure.Domain.ImageEntity.Caching;
using SastImg.Infrastructure.Domain.TagEntity;
using SastImg.Infrastructure.Event;
using SastImg.Infrastructure.Persistence;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using SastImg.Infrastructure.Persistence.TypeConverters;
using SastImg.WebAPI.Configurations;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            SqlMapper.AddTypeHandler(new UriStringConverter());
            services.AddSingleton<DbDataSource>(
                new NpgsqlDataSourceBuilder(connectionString).Build()
            );
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(
                _ => new DbConnectionFactory(connectionString)
            );

            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IGetAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<IGetAlbumRepository, AlbumQueryRepository>();
            services.AddScoped<ISearchAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<IGetRemovedAlbumsRepository, AlbumQueryRepository>();

            services.AddScoped<IGetImagesRepository, ImageQueryRepository>();
            services.AddScoped<IGetImageRepository, ImageQueryRepository>();
            services.AddScoped<ISearchImagesRepository, ImageQueryRepository>();
            services.AddScoped<IGetRemovedImagesRepository, ImageQueryRepository>();

            services.AddScoped<ITagQueryRepository, TagQueryRepository>();

            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();

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
            services.AddScoped<ICache<IEnumerable<AlbumImageDto>>, GetImagesCache>();
            services.AddScoped<ICache<DetailedImageDto>, DetailedImageDtoCache>();
            return services;
        }

        public static IServiceCollection ConfigureExceptionHandlers(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<DbNotFoundExceptionHandler>();
            services.AddExceptionHandler<DomainBusinessRuleInvalidExceptionHandler>();
            services.AddDefaultExceptionHandler();
            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddScoped<IQueryRequestSender, InternalEventBus>();
            services.AddScoped<ICommandRequestSender, InternalEventBus>();
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
            context.HttpContext.Response.WriteAsJsonAsync(
                Responses.TooManyRequests,
                cancellationToken
            );

            return ValueTask.CompletedTask;
        }
    }
}
