using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumCoverUpdatedEventHandler(ICoverStorageManager updater)
    : IDomainEventHandler<AlbumCoverUpdatedEvent>
{
    public async ValueTask Handle(AlbumCoverUpdatedEvent e, CancellationToken cancellationToken)
    {
        if (e.ContainedImage is not null)
        {
            await updater.UpdateWithContainedImageAsync(
                e.Album,
                e.ContainedImage.Value,
                cancellationToken
            );
        }

        if (e.CoverImage is not null)
        {
            await updater.UpdateWithCustomImageAsync(
                e.Album,
                e.CoverImage.Stream,
                cancellationToken
            );
        }

        await updater.DeleteCoverAsync(e.Album, cancellationToken);
    }
}
