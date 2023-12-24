namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(int page, long authorId);

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(int page, long authorId);

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            int page,
            long authorId,
            long requesterId
        );
    }
}
