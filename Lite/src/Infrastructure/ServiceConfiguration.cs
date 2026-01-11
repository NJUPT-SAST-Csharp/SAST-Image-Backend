using System.Data.Common;
using System.Text;
using Application.AlbumServices;
using Application.AlbumServices.Queries;
using Application.CategoryServices;
using Application.CategoryServices.Queries;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Application.Shared;
using Application.UserServices;
using Application.UserServices.Queries;
using Domain;
using Domain.AlbumAggregate;
using Domain.AlbumAggregate.Services;
using Domain.CategoryAggregate;
using Domain.CategoryAggregate.Services;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserAggregate;
using Domain.UserAggregate.Services;
using Infrastructure.AlbumServices.Application;
using Infrastructure.AlbumServices.Domain;
using Infrastructure.CategoryServices.Application;
using Infrastructure.CategoryServices.Domain;
using Infrastructure.ImageServices.Application;
using Infrastructure.Shared.Database;
using Infrastructure.Shared.EventBus;
using Infrastructure.Shared.Storage;
using Infrastructure.UserServices.Application;
using Infrastructure.UserServices.Domain;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Infrastructure;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddScoped<DbConnection>(_ => new NpgsqlConnection(
                configuration.GetConnectionString("Database")
            ))
            .AddDbContext<DomainDbContext>(
                (services, options) =>
                {
                    options.UseNpgsql(services.GetRequiredService<DbConnection>());
                }
            )
            .AddDbContext<QueryDbContext>(
                (services, options) =>
                {
                    options.UseNpgsql(services.GetRequiredService<DbConnection>());
                }
            );

        services
            .AddMediator(options =>
            {
                options.NotificationPublisherType = typeof(ForeachAwaitPublisher);
                options.Assemblies = [DomainAssembly.Assembly];
                options.PipelineBehaviors = [(typeof(UnitOfWorkPostProcessor<,>))];
                options.ServiceLifetime = ServiceLifetime.Scoped;
            })
            .AddScoped<IDomainEventPublisher, EventPublisher>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<StorageOptions>(configuration.GetRequiredSection("Storage"));
        services.AddDistributedMemoryCache();

        return services;
    }

    public static IServiceCollection AddAlbumServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAlbumRepository, AlbumDomainRepository>()
            .AddScoped<IAlbumModelRepository, AlbumModelRepository>()
            .AddScoped<ICategoryExistenceChecker, CategoryExistenceChecker>()
            .AddScoped<ICollaboratorsExistenceChecker, CollaboratorsExistenceChecker>()
            .AddScoped<IAlbumTitleUniquenessChecker, AlbumTitleUniquenessChecker>()
            .AddScoped<ICoverStorageManager, CoverStorageManager>()
            .AddScoped<IAlbumAvailabilityChecker, AlbumAvailabilityChecker>();

        services
            .AddScoped<IQueryRepository<AlbumsQuery, AlbumDto[]>, AlbumQueryRepository>()
            .AddScoped<IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>, AlbumQueryRepository>()
            .AddScoped<
                IQueryRepository<RemovedAlbumsQuery, RemovedAlbumDto[]>,
                AlbumQueryRepository
            >();
        return services;
    }

    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddScoped<IImageModelRepository, ImageModelRepository>();
        services.AddScoped<ILikeModelRepository, LikeModelRepository>();
        services.AddScoped<ISubscribeModelRepository, SubscribeModelRepository>();

        services
            .AddScoped<IQueryRepository<AlbumsQuery, AlbumDto[]>, AlbumQueryRepository>()
            .AddScoped<IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>, AlbumQueryRepository>()
            .AddScoped<
                IQueryRepository<RemovedAlbumsQuery, RemovedAlbumDto[]>,
                AlbumQueryRepository
            >()
            .AddScoped<IQueryRepository<ImagesQuery, ImageDto[]>, ImageQueryRepository>()
            .AddScoped<IQueryRepository<RemovedImagesQuery, ImageDto[]>, ImageQueryRepository>()
            .AddScoped<
                IQueryRepository<DetailedImageQuery, DetailedImage?>,
                ImageQueryRepository
            >();

        services.AddScoped<IImageAvailabilityChecker, ImageAvailabilityChecker>();
        services.AddSingleton<IImageStorageManager, ImageStorageManager>();
        services.AddSingleton<ICompressProcessor, CompressProcessor>();

        return services;
    }

    public static IServiceCollection AddUserServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddScoped<IUserRepository, UserDomainRepository>()
            .AddScoped<IUserRepository, UserDomainRepository>()
            .AddScoped<IUsernameUniquenessChecker, UsernameUniquenessChecker>()
            .AddScoped<IRegistryCodeChecker, RegistryCodeChecker>();

        services
            .AddScoped<IUserModelRepository, UserModelRepository>()
            .AddScoped<IQueryRepository<UserProfileQuery, UserProfileDto?>, UserQueryRepository>()
            .AddScoped<
                IQueryRepository<UsernameExistenceQuery, UsernameExistence>,
                UserQueryRepository
            >();

        services
            .AddMemoryCache()
            .Configure<JwtAuthOptions>(configuration.GetRequiredSection("Auth"))
            .AddSingleton<IPasswordGenerator, PasswordGenerator>()
            .AddSingleton<IPasswordValidator, PasswordValidator>()
            .AddSingleton<IJwtTokenGenerator, JwtTokenManager>();

        services
            .AddSingleton<IAvatarStorageManager, AvatarStorageManager>()
            .AddSingleton<IHeaderStorageManager, HeaderStorageManager>();

        return services;
    }

    public static IServiceCollection AddCategoryServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICategoryRepository, CategoryDomainRepository>()
            .AddScoped<ICategoryNameUniquenessChecker, CategoryNameUniquenessChecker>();

        services
            .AddScoped<ICategoryModelRepository, CategoryModelRepository>()
            .AddScoped<IQueryRepository<CategoriesQuery, CategoryDto[]>, CategoryQueryRepository>();

        return services;
    }

    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var jwtOptions =
            configuration.GetRequiredSection("Auth").Get<JwtAuthOptions>()
            ?? throw new NullReferenceException();

        services
            .AddAuthentication(static options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                string secKey = jwtOptions.SecKey;

                options.TokenValidationParameters = new()
                {
                    NameClaimType = "username",
                    RoleClaimType = "role",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAlgorithms = [jwtOptions.Algorithm],
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy(
                AuthPolicies.Auth,
                policy =>
                    policy.RequireAuthenticatedUser().RequireClaim("id").RequireClaim("username")
            )
            .AddPolicy(
                AuthPolicies.User,
                policy =>
                    policy
                        .RequireAuthenticatedUser()
                        .RequireClaim("id")
                        .RequireClaim("username")
                        .RequireRole(AuthPolicies.User)
            )
            .AddPolicy(
                AuthPolicies.Admin,
                policy =>
                    policy
                        .RequireAuthenticatedUser()
                        .RequireClaim("id")
                        .RequireClaim("username")
                        .RequireRole(AuthPolicies.Admin)
            );

        return services;
    }
}

public readonly struct AuthPolicies
{
    public const string Auth = nameof(Auth);
    public const string User = nameof(Domain.UserAggregate.UserEntity.Role.User);
    public const string Admin = nameof(Domain.UserAggregate.UserEntity.Role.Admin);
}
