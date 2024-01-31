using MediatR;

namespace Shared.Primitives.Query
{
    public interface IQueryRequestHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQueryRequest<TResult> { }
}
