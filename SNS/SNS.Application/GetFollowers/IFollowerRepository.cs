using SNS.Domain;

namespace SNS.Application.GetFollowers
{
    public interface IFollowerRepository
    {
        public Task<IEnumerable<FollowerDto>> GetFollowersAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        );
    }
}
