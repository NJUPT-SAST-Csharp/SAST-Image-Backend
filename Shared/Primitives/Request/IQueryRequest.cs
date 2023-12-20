using MediatR;

namespace Shared.Primitives.Request
{
    public interface IQueryRequest<TResponse> : IRequest<TResponse> { }
}
