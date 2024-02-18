using Account.Domain.UserEntity.Services;
using Account.Infrastructure.Persistence.QueryDatabase;
using Dapper;

namespace Account.Infrastructure.DomainServices
{
    internal sealed class UserUniquenessChecker(IDbConnectionFactory factory)
        : IUserUniquenessChecker
    {
        private readonly IDbConnectionFactory _factory = factory;

        public async Task<bool> CheckEmailExistenceAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = _factory.GetConnection();
            const string sql =
                "SELECT EXISTS ( "
                + "SELECT 1 "
                + "FROM users "
                + "WHERE email ILIKE @email "
                + " );";
            var isExist = await connection.QuerySingleAsync<bool>(sql, new { email });

            return isExist;
        }

        public async Task<bool> CheckUsernameExistenceAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = _factory.GetConnection();
            const string sql =
                "SELECT EXISTS ( "
                + "SELECT 1 "
                + "FROM users "
                + "WHERE username ILIKE @username "
                + " );";
            var isExist = await connection.QuerySingleAsync<bool>(sql, new { username });

            return isExist;
        }
    }
}
