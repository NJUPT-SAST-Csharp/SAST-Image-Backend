using SastImg.Application.AlbumServices.GetAlbums;

namespace SastImg.Application.AlbumServices.SearchAlbums
{
    public interface ISearchAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> SearchAlbumsByAdminAsync(
            long categoryId,
            string title,
            int page
        );
        public Task<IEnumerable<AlbumDto>> SearchAlbumsByUserAsync(
            long categoryId,
            string title,
            int page,
            long requesterId
        );
    }
}
