using MediatR;

namespace Shared.Primitives.Request
{
    public interface IQueryRequestHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQueryRequest<TResult> { }
}
