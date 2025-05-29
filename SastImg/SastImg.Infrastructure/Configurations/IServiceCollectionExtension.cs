using Dapper;
using Exceptions.Configurations;
using Exceptions.ExceptionHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetDetailedAlbum;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
using SastImg.Application.AlbumServices.GetUserAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Application.CategoryServices;
using SastImg.Application.ImageServices;
using SastImg.Application.ImageServices.GetAlbumImages;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.GetUserImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Application.TagServices;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.Categories;
using SastImg.Infrastructure.DomainRepositories;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using SastImg.Infrastructure.Persistence.Storages;
using SastImg.Infrastructure.Persistence.TypeConverters;
using SastImg.Infrastructure.QueryRepositories;
using Shared.Storage.Configurations;

namespace SastImg.Infrastructure.Configurations;

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
        SqlMapper.AddTypeHandler(new UriStringConverter());

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(_ => new DbConnectionFactory(
            connectionString
        ));

        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IGetUserAlbumsRepository, AlbumQueryRepository>();
        services.AddScoped<IGetDetailedAlbumRepository, AlbumQueryRepository>();
        services.AddScoped<IGetAlbumsRepository, AlbumQueryRepository>();
        services.AddScoped<ISearchAlbumsRepository, AlbumQueryRepository>();
        services.AddScoped<IGetRemovedAlbumsRepository, AlbumQueryRepository>();

        services.AddScoped<IGetUserImagesRepository, ImageQueryRepository>();
        services.AddScoped<IGetAlbumImagesRepository, ImageQueryRepository>();
        services.AddScoped<IGetImageRepository, ImageQueryRepository>();
        services.AddScoped<ISearchImagesRepository, ImageQueryRepository>();
        services.AddScoped<IGetRemovedImagesRepository, ImageQueryRepository>();

        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITagQueryRepository, TagQueryRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();

        return services;
    }

    public static IServiceCollection ConfigureExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<DbNotFoundExceptionHandler>();
        services.AddExceptionHandler<NoPermissionExceptionHandler>();
        services.AddDefaultExceptionHandler();
        return services;
    }

    public static IServiceCollection ConfigureStorage(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddStorageClient(options => options.FolderPath = configuration["StoragePath"]!);

        services.TryAddScoped<IImageStorageRepository, ImageStorageRepository>();

        return services;
    }
}
