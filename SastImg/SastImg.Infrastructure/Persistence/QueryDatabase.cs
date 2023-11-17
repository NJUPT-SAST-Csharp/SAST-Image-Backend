using System.Data.Common;
using Dapper;
using SastImg.Application.Services;

namespace SastImg.Infrastructure.Persistence
{
    public class QueryDatabase(DbDataSource source) : IQueryDatabase
    {
        private const int QueryCommandTimeout = 10;

        private DbConnection? _connection = null;

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            _connection = await source.OpenConnectionAsync(cancellationToken);
            var task = _connection.QueryAsync<T>(
                sql,
                parameters,
                commandTimeout: QueryCommandTimeout
            );

            _ = task.ContinueWith(
                async t =>
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                },
                cancellationToken
            );

            return await task;
        }

        public async Task<T> QuerySingle<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        )
        {
            _connection = await source.OpenConnectionAsync(cancellationToken);
            var task = _connection.QuerySingleAsync<T>(
                sql,
                parameters,
                commandTimeout: QueryCommandTimeout
            );

            _ = task.ContinueWith(
                async t =>
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                },
                cancellationToken
            );

            return await task;
        }
    }
}
