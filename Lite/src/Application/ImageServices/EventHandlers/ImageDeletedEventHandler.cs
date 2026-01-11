using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageDeletedEventHandler(
    IImageModelRepository repository,
    IImageStorageManager manager
) : IDomainEventHandler<ImageDeletedEvent>
{
    public async ValueTask Handle(ImageDeletedEvent e, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            repository.DeleteAsync(e.Image, cancellationToken),
            manager.DeleteImageAsync(e.Image, cancellationToken)
        );
    }
}
