using SastImg.Application.Albums.Dtos;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public class GetAlbumsQuery : IQuery<IEnumerable<AlbumDto>> { }
}
