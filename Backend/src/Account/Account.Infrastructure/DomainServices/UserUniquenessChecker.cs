using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Account.Infrastructure.Persistence;
using Dapper;

namespace Account.Infrastructure.DomainServices;

internal sealed class UserUniquenessChecker(IDbConnectionFactory factory)
    : IUsernameUniquenessChecker
{
    private readonly IDbConnectionFactory _factory = factory;

    public async Task<bool> ExistsAsync(
        Username username,
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
        bool isExist = await connection.QuerySingleAsync<bool>(sql, new { username.Value });

        return isExist;
    }
}
