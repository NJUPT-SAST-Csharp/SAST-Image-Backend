using Application.AlbumServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Application;

internal sealed class AlbumModelRepository(QueryDbContext context) : IAlbumModelRepository
{
    public async Task<AlbumModel> GetAsync(
        AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        var album =
            await GetOrDefaultAsync(id, cancellationToken) ?? throw new EntityNotFoundException();

        return album;
    }

    private Task<AlbumModel?> GetOrDefaultAsync(
        AlbumId id,
        CancellationToken cancellationToken = default
    )
    {
        return context
            .Albums.Include(album => album.Images)
            .Include(album => album.Subscribes)
            .FirstOrDefaultAsync(a => a.Id == id.Value, cancellationToken);
    }

    public async Task AddAsync(AlbumModel entity, CancellationToken cancellationToken = default)
    {
        await context.Albums.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(AlbumId id, CancellationToken cancellationToken = default)
    {
        var album = await GetOrDefaultAsync(id, cancellationToken);

        if (album is not null)
            context.Albums.Remove(album);
    }
}
