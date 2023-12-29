using System.Data;

namespace SastImg.Infrastructure.Persistence.QueryDatabase
{
    internal interface IDbConnectionFactory : IDisposable
    {
        public IDbConnection GetConnection();
    }
}
