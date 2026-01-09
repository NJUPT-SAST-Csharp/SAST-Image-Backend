using Dapper;
using Microsoft.Extensions.DependencyInjection;
using SastImg.Application.AlbumAggregate.GetAlbums;
using SastImg.Application.AlbumAggregate.GetDetailedAlbum;
using SastImg.Application.AlbumAggregate.GetRemovedAlbums;
using SastImg.Application.AlbumAggregate.GetUserAlbums;
using SastImg.Application.AlbumAggregate.SearchAlbums;
using SastImg.Application.CategoryServices;
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
using SastImg.Infrastructure.Persistence.TypeConverters;
using SastImg.Infrastructure.QueryRepositories;

namespace SastImg.Infrastructure.Configurations;

public static class IServiceCollectionExtension
{
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
}
