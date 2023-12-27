using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQueryRequest(long albumId, long authorId)
        : IQueryRequest<IEnumerable<ImageDto>>
    {
        public long AlbumId { get; private init; } = albumId;
        public long AuthorId { get; private init; } = authorId;
    }
}
