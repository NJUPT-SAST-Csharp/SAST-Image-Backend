using Dapper;
using SNS.Application.UserServices.GetUser;
using SNS.Domain.UserEntity;
using SNS.Infrastructure.Persistence.QueryDatabase;

namespace SNS.Infrastructure.QueryRepositories
{
    internal sealed class UserQueryRepository(IDbConnectionFactory factory) : IUserQueryRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task<UserDto?> GetUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        )
        {
            using var database = _factory.GetConnection();

            const string query =
                "SELECT "
                + "u.nickname AS Nickname,"
                + "u.biography AS Biography,"
                + "u.avatar AS Avatar,"
                + "u.header AS Header "
                + "FROM users AS u "
                + "WHERE u.id = @Id";

            var user = await database.QuerySingleOrDefaultAsync<UserDto>(
                query,
                new { Id = userId.Value }
            );

            return user;
        }
    }
}
