using MediatR;

namespace Common.Primitives.Query
{
    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult> { }
}
