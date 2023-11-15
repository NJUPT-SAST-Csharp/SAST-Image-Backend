using Microsoft.EntityFrameworkCore;
using SastImg.Domain;
using SastImg.Domain.Albums;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Domain.Albums
{
    internal class AlbumRepository(SastImgDbContext dbContext, IUnitOfWork unitOfWork)
        : IAlbumRepository
    {
        private readonly SastImgDbContext _dbContext = dbContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Album

        public Task<long> CreateAlbumAsync(
            long userId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        )
        {
            var album = Album.CreateNewAlbum(userId, title, description, accessibility);
            _dbContext.Albums.AddAsync(album, cancellationToken);
            return Task.FromResult(album.Id);
        }

        public Task DeleteAllRemovedAlbumsAsync(CancellationToken cancellationToken = default)
        {
            var albums = _dbContext.Albums.Where(album => album.IsRemoved);
            _dbContext.RemoveRange(albums);
            return Task.CompletedTask;
        }

        public Task RemoveAlbumAsync(long albumId, CancellationToken cancellationToken = default)
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is { })
            {
                album.Remove();
            }
            return Task.CompletedTask;
        }

        public Task RestoreAlbumAsync(long albumId, CancellationToken cancellationToken)
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is { })
            {
                album.Restore();
            }
            return Task.CompletedTask;
        }

        public Task UpdateAlbumAsync(
            long albumId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        )
        {
            var album = _dbContext.Albums.Where(album => album.Id == albumId).FirstOrDefault();
            if (album is { })
            {
                album.UpdateAlbumInfo(title, description, accessibility);
            }
            return Task.CompletedTask;
        }

        #endregion

        #region Image


        public Task DeleteAllRemovedImagesAsync(CancellationToken cancellationToken = default)
        {
            var images = _dbContext
                .Albums
                .SelectMany(album => album.Images)
                .Where(image => image.IsRemoved);
            _dbContext.RemoveRange(images);
            return Task.CompletedTask;
        }

        public async Task AddAlbumImageAsync(
            long albumId,
            string title,
            string description,
            Uri imageUri,
            IEnumerable<long> tags,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _dbContext
                .Albums
                .Where(album => album.Id == albumId)
                .FirstOrDefaultAsync(cancellationToken);
            if (album is { })
            {
                album.AddImage(title, imageUri, description);
            }
        }

        public async Task RemoveImageFromAlbumAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _dbContext
                .Albums
                .Where(album => album.Id == albumId)
                .FirstOrDefaultAsync();
            if (album is { })
            {
                album.RemoveImage(imageId);
            }
        }

        public async Task UpdateAlbumImageAsync(
            long albumId,
            long imageId,
            string title,
            string description,
            IEnumerable<long> tags,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _dbContext
                .Albums
                .Where(album => album.Id == albumId)
                .FirstOrDefaultAsync(cancellationToken);
            if (album is { })
            {
                album.UpdateImage(imageId, title, description, tags);
            }
        }

        public async Task RestoreRemovedAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _dbContext
                .Albums
                .Where(album => album.Id == albumId && album.IsRemoved)
                .FirstOrDefaultAsync(cancellationToken);
            if (album is { })
            {
                album.Restore();
            }
        }

        public async Task RestoreRemovedAlbumImageAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _dbContext
                .Albums
                .Include(album => album.Images)
                .Where(album => album.Id == albumId && album.IsRemoved)
                .FirstOrDefaultAsync(cancellationToken);
            if (album is { })
            {
                album.RestoreImage(imageId);
            }
        }

        #endregion
    }
}
