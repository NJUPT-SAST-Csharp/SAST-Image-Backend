using System.Data.Common;
using Dapper;
using SastImg.Application.Services;

namespace SastImg.Infrastructure.Persistence
{
    public class QueryDatabase(DbDataSource source) : IQueryDatabase
    {
        private const int QueryCommandTimeout = 10;

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = await source.OpenConnectionAsync(cancellationToken);
            return await connection.QueryAsync<T>(
                sql,
                parameters,
                commandTimeout: QueryCommandTimeout
            );
        }

        public async Task<T> QuerySingle<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            using var connection = await source.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleAsync<T>(
                sql,
                parameters,
                commandTimeout: QueryCommandTimeout
            );
        }
    }
}
