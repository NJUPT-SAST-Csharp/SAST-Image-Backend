namespace SastImg.Application.Query.Album.GetAlbums
{
    public interface IGetAlbumsAnonymousCache
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsAsync();

        public Task SetAlbumsAsync(IEnumerable<AlbumDto> albums);
    }
}
