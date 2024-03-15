using Account.Application.UserServices;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;
using Account.Infrastructure.Persistence;
using Dapper;

namespace Account.Infrastructure.ApplicationServices
{
    internal sealed class UserQueryRepository(IDbConnectionFactory factory) : IUserQueryRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task<UserBriefInfoDto?> GetUserBriefInfoAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = _factory.GetConnection();

            const string sql =
                "SELECT "
                + "username, nickname, avatar "
                + "FROM users "
                + "WHERE username = @username";

            var result = await connection.QueryFirstOrDefaultAsync<UserBriefInfoDto>(
                sql,
                new { username }
            );

            return result;
        }

        public async Task<UserDetailedInfoDto?> GetUserDetailedInfoAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = _factory.GetConnection();

            const string sql =
                "SELECT "
                + "username, nickname, biography, avatar, header, birthday, website, id "
                + "FROM users "
                + "WHERE username = @username";

            var result = await connection.QueryFirstOrDefaultAsync<UserDetailedInfoDto>(
                sql,
                new { username }
            );

            return result;
        }
    }
}
