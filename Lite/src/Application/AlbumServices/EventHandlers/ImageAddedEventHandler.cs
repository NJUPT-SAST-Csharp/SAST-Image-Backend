using Application.ImageServices;
using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class ImageAddedEventHandler(
    IAlbumModelRepository repository,
    IImageStorageManager manager
) : IDomainEventHandler<ImageAddedEvent>
{
    public async ValueTask Handle(ImageAddedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);

        await manager.StoreImageAsync(e.ImageId, e.ImageFile.Stream, cancellationToken);

        album.ImageAdded(e);
    }
}
