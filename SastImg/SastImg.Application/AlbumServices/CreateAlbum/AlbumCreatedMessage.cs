using Messenger;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    internal readonly struct AlbumCreatedMessage(UserId authorId, AlbumId albumId) : IMessage
    {
        public readonly long AuthorId { get; } = authorId.Value;
        public readonly long AlbumId { get; } = albumId.Value;
    }
}
