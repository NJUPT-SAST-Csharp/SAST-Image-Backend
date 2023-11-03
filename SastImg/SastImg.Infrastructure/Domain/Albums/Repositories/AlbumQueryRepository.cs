using Dapper;
using SastImg.Application.Albums;
using SastImg.Application.Albums.Dtos;
using SastImg.Application.Albums.Images.Dtos;
using SastImg.Infrastructure.Persistence;
using System.Data;

namespace SastImg.Infrastructure.Domain.Albums.Repositories
{
    public class AlbumQueryRepository : IAlbumQueryRepository
    {
        private readonly IDbConnection _connection;

        public AlbumQueryRepository(IDbConnectionProvider connectionProvider)
        {
            _connection = connectionProvider.DbConnection;
        }

        public Task<AlbumDetailsDto?> GetAlbumDetailsAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            string sql = "";
            return _connection.QueryFirstOrDefaultAsync<AlbumDetailsDto>(sql);
        }

        public Task<IEnumerable<ImageDto>> GetAlbumImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> GetAlbumRemovedImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<ImageDetailsDto> GetImageDetailsAsync(
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> GetImagesByCategoryAsync(
            long category,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> GetImagesByTagAsync(
            IEnumerable<long> tags,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlbumDto>> GetRemovedAlbumsAsync(
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlbumDto>> GetUserAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlbumDto>> GetUserRemovedAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
