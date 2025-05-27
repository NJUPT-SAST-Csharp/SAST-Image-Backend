using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.DomainRepositories;

public sealed class AlbumRepository(SastImgDbContext context) : IAlbumRepository
{
    private readonly SastImgDbContext _context = context;

    public async Task<AlbumId> AddAlbumAsync(
        Album album,
        CancellationToken cancellationToken = default
    )
    {
        var a = await _context.Albums.AddAsync(album, cancellationToken);
        return a.Entity.Id;
    }

    public async Task<Album> GetAlbumAsync(
        AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        var album = await _context
            .Albums.Include("_images")
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (album is null)
        {
            throw new DbNotFoundException(nameof(Album), id.Value.ToString());
        }

        return album;
    }
}
