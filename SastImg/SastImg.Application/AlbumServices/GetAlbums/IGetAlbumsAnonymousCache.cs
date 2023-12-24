namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetAlbumsAnonymousCache
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsAsync(int page, long authorId);

        public Task RemoveAlbumsAsync(long authorId);
    }
}
