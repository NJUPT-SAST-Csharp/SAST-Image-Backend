using Microsoft.EntityFrameworkCore;
using SastImg.Domain;
using SastImg.Domain.Albums;
using SastImg.Domain.Albums.Images;
using SastImg.Domain.Albums.Repositories;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Domain.Albums.Repositories
{
    public class AlbumQueryRepository : IAlbumQueryRepository
    {
        private readonly SastImgDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public AlbumQueryRepository(SastImgDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        #region Album

        public Task<Album?> GetAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Albums
                .Include(album => album.Images)
                .Where(album => !album.IsRemoved && album.Id == albumId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Album[]> GetUserAlbumsAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Albums
                .Where(album => !album.IsRemoved && album.AuthorId == albumId)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Album[]> GetRemovedAlbumsAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Albums
                .Where(album => album.IsRemoved)
                .ToArrayAsync(cancellationToken);
        }

        public Task<Album[]> GetUserRemovedAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext.Albums
                .Where(album => album.IsRemoved && album.AuthorId == userId)
                .ToArrayAsync(cancellationToken);
        }

        #endregion

        #region Image

        public Task<Image[]> GetAlbumImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetImageAsync(
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<Image[]> GetAlbumRemovedImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
