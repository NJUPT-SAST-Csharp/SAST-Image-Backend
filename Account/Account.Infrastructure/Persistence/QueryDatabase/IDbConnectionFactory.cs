using System.Data;

namespace Account.Infrastructure.Persistence.QueryDatabase
{
    internal interface IDbConnectionFactory : IDisposable
    {
        public IDbConnection GetConnection();
    }
}
