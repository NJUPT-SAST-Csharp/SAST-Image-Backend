using MediatR;

namespace Shared.Primitives.Query
{
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}
