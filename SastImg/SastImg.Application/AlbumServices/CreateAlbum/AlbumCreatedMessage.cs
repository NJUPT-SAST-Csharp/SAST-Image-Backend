using Messenger;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    public readonly struct AlbumCreatedMessage(in UserId authorId, in AlbumId albumId) : IMessage
    {
        public readonly long AuthorId { get; } = authorId.Value;
        public readonly long AlbumId { get; } = albumId.Value;
    }
}
