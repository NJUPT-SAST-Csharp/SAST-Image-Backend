using System.Data;

namespace SNS.Infrastructure.Query
{
    internal interface IDbConnectionFactory : IDisposable
    {
        public IDbConnection GetConnection();
    }
}
