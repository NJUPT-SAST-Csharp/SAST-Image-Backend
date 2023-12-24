using System.Data;

namespace SastImg.Infrastructure.Persistence.QueryDatabase
{
    internal interface IDbConnectionFactory
    {
        public IDbConnection GetConnection();
    }
}
