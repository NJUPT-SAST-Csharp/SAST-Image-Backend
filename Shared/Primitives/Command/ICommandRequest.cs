using MediatR;

namespace Primitives.Command
{
    public interface ICommandRequest : IRequest { }

    public interface ICommandRequest<TResponse> : IRequest<TResponse> { }
}
