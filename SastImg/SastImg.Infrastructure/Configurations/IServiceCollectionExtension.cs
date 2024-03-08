using System.Data.Common;
using System.Reflection;
using Dapper;
using Exceptions.Configurations;
using Exceptions.ExceptionHandlers;
using Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Npgsql;
using Primitives;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetDetailedAlbum;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
using SastImg.Application.AlbumServices.GetUserAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Application.CategoryServices;
using SastImg.Application.CategoryServices.GetAllCategory;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Application.SeedWorks;
using SastImg.Application.TagServices;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.Categories;
using SastImg.Domain.TagEntity;
using SastImg.Infrastructure.Caching;
using SastImg.Infrastructure.DomainRepositories;
using SastImg.Infrastructure.Event;
using SastImg.Infrastructure.EventBus;
using SastImg.Infrastructure.Persistence;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using SastImg.Infrastructure.Persistence.TypeConverters;
using SastImg.Infrastructure.QueryRepositories;
using SastImg.WebAPI.Configurations;
using Shared.Storage.Configurations;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Configurations
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
            services.AddScoped<IGetUserAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<IGetDetailedAlbumRepository, AlbumQueryRepository>();
            services.AddScoped<IGetAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<ISearchAlbumsRepository, AlbumQueryRepository>();
            services.AddScoped<IGetRemovedAlbumsRepository, AlbumQueryRepository>();

            services.AddScoped<IGetImagesRepository, ImageQueryRepository>();
            services.AddScoped<IGetImageRepository, ImageQueryRepository>();
            services.AddScoped<ISearchImagesRepository, ImageQueryRepository>();
            services.AddScoped<IGetRemovedImagesRepository, ImageQueryRepository>();

            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagQueryRepository, TagQueryRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
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
            services.AddScoped<ICache<DetailedAlbumDto>, GetAlbumCache>();
            services.AddScoped<ICache<IEnumerable<AlbumImageDto>>, GetImagesCache>();
            services.AddScoped<ICache<DetailedImageDto>, DetailedImageDtoCache>();
            services.AddScoped<ICache<IEnumerable<CategoryDto>>, GetAllCategoriesCache>();
            return services;
        }

        public static IServiceCollection ConfigureExceptionHandlers(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<DbNotFoundExceptionHandler>();
            services.AddExceptionHandler<NoPermissionExceptionHandler>();
            services.AddExceptionHandler<DomainBusinessRuleInvalidExceptionHandler>();
            services.AddDefaultExceptionHandler();
            return services;
        }

        public static IServiceCollection ConfigureEventBus(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var config = configuration.GetSection("EventBus");

            services.AddCap(x =>
            {
                x.UseEntityFramework<SastImgDbContext>();
                x.UseRabbitMQ(options =>
                {
                    options.Port = config.GetValue<int>("Port");
                    options.HostName = config["HostName"]!;
                    options.UserName = config["UserName"]!;
                    options.Password = config["Password"]!;
                });
            });

            services.AddScoped<IMessagePublisher, ExternalEventBus>();

            return services;
        }

        public static IServiceCollection ConfigureStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.ConfigureImageStorage(configuration);
            services.TryAddScoped<IImageStorageRepository, ImageStorageRepository>();

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
    }
}
