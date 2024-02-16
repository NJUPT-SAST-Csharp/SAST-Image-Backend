using System.Data;

namespace SNS.Infrastructure.Persistence.QueryDatabase
{
    internal interface IDbConnectionFactory : IDisposable
    {
        public IDbConnection GetConnection();
    }
}
