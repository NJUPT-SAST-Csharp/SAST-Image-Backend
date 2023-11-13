using System.Data;
using Npgsql;

namespace SastImg.Infrastructure.Persistence
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly IDbConnection _connection;
        private readonly NpgsqlDataSource _dbSource;

        public DbConnectionProvider(string connectionString)
        {
            _dbSource = new NpgsqlDataSourceBuilder(connectionString).Build();
            _connection = _dbSource.OpenConnection();
        }

        public IDbConnection DbConnection =>
            _connection.State == ConnectionState.Closed ? _dbSource.OpenConnection() : _connection;
    }
}
