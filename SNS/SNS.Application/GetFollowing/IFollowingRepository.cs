using Identity;

namespace SNS.Application.GetFollowing;

public interface IFollowingRepository
{
    public Task<IEnumerable<FollowingDto>> GetFollowingAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}
