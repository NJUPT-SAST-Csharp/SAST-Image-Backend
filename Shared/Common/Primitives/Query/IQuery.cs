using MediatR;

namespace Common.Primitives.Query
{
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}
