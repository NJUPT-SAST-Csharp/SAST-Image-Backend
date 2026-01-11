using Application.Query;

namespace Application.Shared;

public interface IQueryRepository<TQuery, TQueryResponse>
    where TQuery : IQuery<TQueryResponse>
{
    public Task<TQueryResponse> GetOrDefaultAsync(
        TQuery query,
        CancellationToken cancellationToken = default
    );
}
