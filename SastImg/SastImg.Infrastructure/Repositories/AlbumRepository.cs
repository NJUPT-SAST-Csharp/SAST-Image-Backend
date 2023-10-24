using Microsoft.EntityFrameworkCore;
using SastImg.Domain;
using SastImg.Domain.Albums;
using SastImg.Domain.Albums.Images;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SastImgDbContext _dbContext;
        private readonly IUnitOfWork _unit;

        public AlbumRepository(SastImgDbContext dbContext, IUnitOfWork unit)
        {
            _dbContext = dbContext;
            _unit = unit;
        }

        #region Album
        public Task<long> CreateAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        )
        {
            _dbContext.Albums.AddAsync(album);
            _unit.CommitChangesAsync(cancellationToken);
            return Task.FromResult(album.AuthorId);
        }

        public Task<int> DeleteAllRemovedAlbumsAsync(CancellationToken cancellationToken = default)
        {
            var albums = _dbContext.Albums.Where(album => album.IsRemoved);
            _dbContext.RemoveRange(albums);
            return _unit.CommitChangesAsync(cancellationToken);
        }

        public Task<int> DeleteAllRemovedImagesAsync(CancellationToken cancellationToken = default)
        {
            var images = _dbContext.Albums
                .SelectMany(album => album.Images)
                .Where(image => image.IsRemoved);
            _dbContext.RemoveRange(images);
            return _unit.CommitChangesAsync(cancellationToken);
        }

        public Task<Album?> GetAlbumByIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Albums
                .Include(album => album.Images)
                .Where(album => album.Id == albumId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Album[]> GetAlbumsByAuthorIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Albums
                .Where(album => album.AuthorId == albumId)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Album[]> GetRemovedAlbumsAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Albums
                .Where(album => album.IsRemoved)
                .ToArrayAsync(cancellationToken);
        }

        public Task RemoveAlbumByIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is not null)
            {
                album.Remove();
                return _unit.CommitChangesAsync(cancellationToken);
            }
            else
                return Task.CompletedTask;
        }

        public Task RestoreAlbumByIdAsync(long albumId, CancellationToken cancellationToken)
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is not null)
            {
                album.Restore();
                return _unit.CommitChangesAsync(cancellationToken);
            }
            else
                return Task.CompletedTask;
        }

        public Task UpdateAlbumInfoAsync(
            long albumId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        )
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is not null)
            {
                album.UpdateAlbumInfo(title, description, accessibility);
                return _unit.CommitChangesAsync(cancellationToken);
            }
            else
                return Task.CompletedTask;
        }

        #endregion

        #region Image

        public async Task AddImageToAlbumAsync(
            long albumId,
            Image image,
            CancellationToken cancellationToken = default
        )
        {
            var album = await GetAlbumByIdAsync(albumId, cancellationToken);
            if (album is { })
            {
                // TODO: Change the parameters.
                album.AddImage();
                _ = _unit.CommitChangesAsync(cancellationToken);
            }
        }

        public async Task RemoveImageFromAlbumAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await GetAlbumByIdAsync(albumId, cancellationToken);
            if (album is { })
            {
                album.RemoveImageById(imageId);
                _ = _unit.CommitChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateImageAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await GetAlbumByIdAsync(albumId, cancellationToken);
        }

        #endregion
    }
}
