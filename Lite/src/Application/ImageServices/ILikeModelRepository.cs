using Domain.AlbumAggregate.ImageEntity;
using Domain.UserAggregate.UserEntity;

namespace Application.ImageServices;

public interface ILikeModelRepository
{
    public Task AddAsync(LikeModel model, CancellationToken cancellationToken);

    public Task DeleteAsync(ImageId image, UserId user, CancellationToken cancellationToken);
}
