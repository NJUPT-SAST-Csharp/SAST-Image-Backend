using Primitives.Command;
using SNS.Domain.AlbumEntity;
using SNS.Domain.UserEntity;

namespace SNS.Application.AlbumServices.AddAlbum
{
    public sealed class AddAlbumCommand(long albumId, long authorId) : ICommandRequest
    {
        public AlbumId AlbumId { get; } = new(albumId);
        public UserId AuthorId { get; } = new(authorId);
    }
}
