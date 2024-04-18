using Dapper;
using SNS.Application.GetFollowCount;
using SNS.Application.GetFollowers;
using SNS.Application.GetFollowing;
using SNS.Domain;

namespace SNS.Infrastructure.Query
{
    internal sealed class FollowRepository(IDbConnectionFactory factory)
        : IFollowerRepository,
            IFollowingRepository,
            IFollowCountRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task<FollowCountDto> GetFollowCountAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "(SELECT COUNT(*) FROM follows WHERE follower = @UserId) AS Following, "
                + "(SELECT COUNT(*) FROM follows WHERE following = @UserId) AS Follower";

            using var connection = _factory.GetConnection();

            return await connection
                    .QueryFirstOrDefaultAsync<FollowCountDto>(sql, new { UserId = userId.Value })
                    .WaitAsync(cancellationToken) ?? new FollowCountDto();
        }

        public async Task<IEnumerable<FollowerDto>> GetFollowersAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT follower AS Id " + "FROM follows " + "WHERE following = @UserId";

            using var connection = _factory.GetConnection();

            var followers = await connection
                .QueryAsync<FollowerDto>(sql, new { UserId = userId.Value })
                .WaitAsync(cancellationToken);

            return followers;
        }

        public async Task<IEnumerable<FollowingDto>> GetFollowingAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT following AS Id " + "FROM follows " + "WHERE follower = @UserId";

            using var connection = _factory.GetConnection();

            var following = await connection
                .QueryAsync<FollowingDto>(sql, new { UserId = userId.Value })
                .WaitAsync(cancellationToken);

            return following;
        }
    }
}
