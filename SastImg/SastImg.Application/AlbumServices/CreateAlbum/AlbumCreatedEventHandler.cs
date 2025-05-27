using Mediator;
using Messenger;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.AlbumServices.CreateAlbum;

internal sealed class AlbumCreatedEventHandler(IMessagePublisher messenger)
    : IDomainEventHandler<AlbumCreatedDomainEvent>
{
    private readonly IMessagePublisher _messenger = messenger;

    public async ValueTask Handle(
        AlbumCreatedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        await _messenger.PublishAsync(
            "sastimg.album.created",
            new AlbumCreatedMessage(notification),
            cancellationToken
        );
    }
}
