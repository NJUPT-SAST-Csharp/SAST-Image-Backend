using Mediator;
using Messenger;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.ImageServices.AddImage;

public sealed class ImageAddedEventHandler(IMessagePublisher publisher)
    : IDomainEventHandler<ImageAddedDomainEvent>
{
    private readonly IMessagePublisher _publisher = publisher;

    public async ValueTask Handle(
        ImageAddedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        await _publisher.PublishAsync(
            "sastimg.image.added",
            new ImageAddedMessage(notification),
            cancellationToken
        );
    }
}
