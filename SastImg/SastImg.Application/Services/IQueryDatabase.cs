namespace SastImg.Application.Services
{
    public interface IQueryDatabase
    {
        public Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        );

        public Task<T> QuerySingle<T>(
            string sql,
            object? parameters = null,
            CancellationToken cancellationToken = default
        );
    }
}
