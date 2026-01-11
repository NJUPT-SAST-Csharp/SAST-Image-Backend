using Domain.AlbumAggregate.AlbumEntity;
using Domain.UserAggregate.UserEntity;

namespace Application.AlbumServices;

public interface ISubscribeModelRepository
{
    public Task<SubscribeModel> GetAsync(
        AlbumId albumId,
        UserId userId,
        CancellationToken cancellationToken
    );

    public Task AddAsync(SubscribeModel model, CancellationToken cancellationToken);

    public Task DeleteAsync(AlbumId album, UserId user, CancellationToken cancellationToken);
}
