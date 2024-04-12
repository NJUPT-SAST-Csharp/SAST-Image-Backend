namespace SNS.Domain.Follows
{
    internal interface IFollowManager
    {
        Task<Follow> GetFollowAsync(
            UserId follower,
            UserId target,
            CancellationToken cancellationToken = default
        );
        Task FollowAsync(
            UserId follower,
            UserId target,
            CancellationToken cancellationToken = default
        );
        Task UnfollowAsync(Follow follow, CancellationToken cancellationToken = default);
    }
}
