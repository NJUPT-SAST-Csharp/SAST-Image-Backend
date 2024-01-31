using MediatR;

namespace Shared.Primitives.Query
{
    public interface IQueryRequest<TResponse> : IRequest<TResponse> { }
}
