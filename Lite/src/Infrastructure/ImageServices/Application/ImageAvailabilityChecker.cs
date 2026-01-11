using Application.ImageServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageAvailabilityChecker(QueryDbContext context) : IImageAvailabilityChecker
{
    private readonly QueryDbContext _context = context;

    public async Task<bool> CheckAsync(
        ImageId id,
        Actor actor,
        CancellationToken cancellationToken = default
    )
    {
        var image = await _context
            .Images.AsNoTracking()
            .Where(i => i.Id == id.Value)
            .Select(i => new
            {
                i.AuthorId,
                i.Status,
                i.Collaborators,
                i.AccessLevel,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (image is null)
            return false;
        if (
            image.AuthorId == actor.Id.Value
            || actor.IsAdmin
            || image.Collaborators.Contains(actor.Id.Value)
        )
            return true;
        if (image.Status != ImageStatusValue.Available)
            return false;

        return image.AccessLevel switch
        {
            >= AccessLevelValue.PublicReadOnly => true,
            >= AccessLevelValue.AuthReadOnly => actor.IsAuthenticated,
            AccessLevelValue.Private => false,

            _ => false,
        };
    }
}
