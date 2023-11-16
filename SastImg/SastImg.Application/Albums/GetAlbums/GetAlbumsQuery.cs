using SastImg.Application.Albums.Dtos;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public class GetAlbumsQuery(int page) : IQuery<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
    }
}
