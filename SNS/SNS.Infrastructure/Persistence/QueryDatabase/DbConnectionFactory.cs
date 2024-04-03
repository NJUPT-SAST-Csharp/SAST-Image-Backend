using System.Data;
using Npgsql;

namespace SNS.Infrastructure.Persistence.QueryDatabase
{
    internal sealed class DbConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private readonly string _connectionString = connectionString;
        private IDbConnection? _connection = null;

        public void Dispose()
        {
            if (_connection is not null && _connection.State == ConnectionState.Open)
            {
                _connection.Dispose();
            }
        }

        public IDbConnection GetConnection()
        {
            if (_connection is null || _connection.State != ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }
    }
}
