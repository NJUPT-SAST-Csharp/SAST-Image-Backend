using Messenger;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.ImageServices.AddImage;

internal readonly struct ImageAddedMessage(ImageAddedDomainEvent domainEvent) : IMessage
{
    public readonly long ImageId { get; } = domainEvent.ImageId.Value;
    public readonly long AlbumId { get; } = domainEvent.AlbumId.Value;
    public readonly long AuthorId { get; } = domainEvent.AuthorId.Value;
}
