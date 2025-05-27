using Identity;

namespace SastImg.Application.ImageServices.GetUserImages;

public interface IGetUserImagesRepository
{
    public Task<IEnumerable<UserImageDto>> GetUserImagesByAdminAsync(
        UserId userId,
        int page,
        CancellationToken cancellationToken = default
    );
    public Task<IEnumerable<UserImageDto>> GetUserImagesByUserAsync(
        UserId userId,
        UserId requesterId,
        int page,
        CancellationToken cancellationToken = default
    );
}
