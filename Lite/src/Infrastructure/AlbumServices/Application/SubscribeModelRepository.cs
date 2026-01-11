using Application.AlbumServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Database;

namespace Infrastructure.AlbumServices.Application;

internal sealed class SubscribeModelRepository(QueryDbContext context) : ISubscribeModelRepository
{
    private readonly QueryDbContext _context = context;

    public async Task AddAsync(SubscribeModel entity, CancellationToken cancellationToken = default)
    {
        await _context.Subscribes.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(
        AlbumId album,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        var subscribe = await GetOrDefaultAsync(album, user, cancellationToken);

        if (subscribe is not null)
            _context.Subscribes.Remove(subscribe);
    }

    public async Task<SubscribeModel> GetAsync(
        AlbumId album,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        var subscribe =
            await GetOrDefaultAsync(album, user, cancellationToken)
            ?? throw new EntityNotFoundException();

        return subscribe;
    }

    private Task<SubscribeModel?> GetOrDefaultAsync(
        AlbumId album,
        UserId user,
        CancellationToken cancellationToken = default
    )
    {
        return _context.Subscribes.FindAsync([album.Value, user.Value], cancellationToken).AsTask();
    }
}
