using Npgsql;
using System.Data;

namespace SastImg.Infrastructure.Persistence
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        // TODO: Replace the dbConnectionString.
        private readonly string _dbConnectionString = string.Empty;
        private IDbConnection _connection;
        private readonly NpgsqlDataSource _dbSource;

        public DbConnectionProvider()
        {
            _dbSource = new NpgsqlDataSourceBuilder(_dbConnectionString).Build();
            _connection = _dbSource.OpenConnection();
        }

        public IDbConnection DbConnection =>
            _connection.State == ConnectionState.Closed ? _dbSource.OpenConnection() : _connection;
    }
}
