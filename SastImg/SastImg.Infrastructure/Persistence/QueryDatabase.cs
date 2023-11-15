using System.Data;
using Dapper;
using Npgsql;
using SastImg.Application.Services;

namespace SastImg.Infrastructure.Persistence
{
    public class QueryDatabase : IQueryDatabase
    {
        private const int QueryCommandTimeout = 10;

        private readonly IDbConnection _connection;
        private readonly NpgsqlDataSource _dbSource;

        private IDbConnection Connection =>
            _connection.State == ConnectionState.Closed ? _dbSource.OpenConnection() : _connection;

        public QueryDatabase(string connectionString)
        {
            _dbSource = new NpgsqlDataSourceBuilder(connectionString).Build();
            _connection = _dbSource.OpenConnection();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            return Connection.QueryAsync<T>(sql, parameters, commandTimeout: QueryCommandTimeout);
        }

        public Task<T> QuerySingle<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            return Connection.QuerySingleAsync<T>(
                sql,
                parameters,
                commandTimeout: QueryCommandTimeout
            );
        }
    }
}
