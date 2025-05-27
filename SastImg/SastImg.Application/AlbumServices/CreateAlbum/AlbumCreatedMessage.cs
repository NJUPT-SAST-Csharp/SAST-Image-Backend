using Messenger;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

namespace SastImg.Application.AlbumServices.CreateAlbum;

internal readonly struct AlbumCreatedMessage(AlbumCreatedDomainEvent domainEvent) : IMessage
{
    public readonly long AuthorId { get; } = domainEvent.AuthorId.Value;
    public readonly long AlbumId { get; } = domainEvent.AlbumId.Value;
}
