using SastImg.Application.Albums.Dtos;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public sealed class GetAlbumsQuery(int page, long userId) : IQuery<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public long UserId { get; } = userId;
    }
}
