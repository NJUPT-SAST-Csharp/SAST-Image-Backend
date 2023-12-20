using Shared.Primitives.Request;

namespace SastImg.Application.Query.Album.GetAlbums
{
    public class GetAlbumsQueryRequest(int page, long authorId, RequesterInfo requester)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public long AuthorId { get; } = authorId;
        public RequesterInfo Requester { get; } = requester;
    }
}
