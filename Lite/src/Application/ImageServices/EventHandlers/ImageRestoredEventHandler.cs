using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageRestoredEventHandler(IImageModelRepository repository)
    : IDomainEventHandler<ImageRestoredEvent>
{
    public async ValueTask Handle(ImageRestoredEvent e, CancellationToken cancellationToken)
    {
        var image = await repository.GetAsync(e.Image, cancellationToken);

        image.Restore(e);
    }
}
