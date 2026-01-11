using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Core.Event;

namespace Application.ImageServices.EventHandlers;

internal sealed class ImageTagUpdatedEventHandler(IImageModelRepository repository)
    : IDomainEventHandler<ImageTagsUpdatedEvent>
{
    public async ValueTask Handle(ImageTagsUpdatedEvent e, CancellationToken cancellationToken)
    {
        var image = await repository.GetAsync(e.Id, cancellationToken);

        image.UpdateTags(e);
    }
}
