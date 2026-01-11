using Application.AlbumServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Application;

internal sealed class AlbumAvailabilityChecker(QueryDbContext context) : IAlbumAvailabilityChecker
{
    private readonly QueryDbContext _context = context;

    public async Task<bool> CheckAsync(
        AlbumId albumId,
        Actor actor,
        CancellationToken cancellationToken
    )
    {
        var album = await _context
            .Albums.AsNoTracking()
            .Where(a => a.Id == albumId.Value)
            .Select(a => new
            {
                a.AccessLevel,
                a.AuthorId,
                a.Status,
                a.Collaborators,
            })
            .FirstOrDefaultAsync(cancellationToken);

        long actorId = actor.Id.Value;

        if (album is null)
            return false;
        if (
            actor.IsAdmin
            || album.AuthorId == actor.Id.Value
            || album.Collaborators.Contains(actor.Id.Value)
        )
            return true;
        if (album.Status == AlbumStatusValue.Removed)
            return false;

        return album.AccessLevel switch
        {
            >= AccessLevelValue.PublicReadOnly => true,
            >= AccessLevelValue.AuthReadOnly => actor.IsAuthenticated,
            AccessLevelValue.Private => false,

            _ => false,
        };
    }
}
