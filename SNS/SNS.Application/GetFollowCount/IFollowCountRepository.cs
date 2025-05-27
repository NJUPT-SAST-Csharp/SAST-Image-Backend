using Identity;

namespace SNS.Application.GetFollowCount;

public interface IFollowCountRepository
{
    public Task<FollowCountDto> GetFollowCountAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}
