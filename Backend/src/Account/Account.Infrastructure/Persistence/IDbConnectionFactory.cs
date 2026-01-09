using System.Data;

namespace Account.Infrastructure.Persistence;

internal interface IDbConnectionFactory : IDisposable
{
    public IDbConnection GetConnection();
}
