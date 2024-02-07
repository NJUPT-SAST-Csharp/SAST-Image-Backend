using Messenger;
using Primitives.DomainEvent;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.ImageServices.AddImage
{
    internal class ImageAddedDomainEventHandler(IMessagePublisher publisher)
        : IDomainEventHandler<ImageAddedDomainEvent>
    {
        private readonly IMessagePublisher _publisher = publisher;

        public async Task Handle(
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
}
