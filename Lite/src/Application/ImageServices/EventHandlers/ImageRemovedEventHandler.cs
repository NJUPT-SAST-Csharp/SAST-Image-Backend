using Domain.AlbumAggregate.Events;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageRemovedEventHandler(IImageModelRepository repository)
    : IDomainEventHandler<ImageRemovedEvent>
{
    public async ValueTask Handle(ImageRemovedEvent e, CancellationToken cancellationToken)
    {
        var image = await repository.GetAsync(e.Image, cancellationToken);

        image.Remove(e);
    }
}
