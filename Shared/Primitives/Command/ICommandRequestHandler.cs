using MediatR;

namespace Primitives.Command
{
    public interface ICommandRequestHandler<TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommandRequest<TResponse> { }

    public interface ICommandRequestHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommandRequest { }
}
