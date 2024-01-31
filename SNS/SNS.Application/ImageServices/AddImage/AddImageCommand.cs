using Primitives.Command;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserEntity;

namespace SNS.Application.ImageServices.AddImage
{
    public sealed class AddImageCommand(long imageId, long authorId, long albumId) : ICommandRequest
    {
        public ImageId ImageId { get; } = new(imageId);
        public UserId AuthorId { get; } = new(authorId);
        public AlbumId AlbumId { get; } = new(albumId);
    }
}
