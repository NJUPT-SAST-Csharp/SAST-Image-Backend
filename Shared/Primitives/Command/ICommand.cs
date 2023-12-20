using MediatR;

namespace Primitives.Command
{
    public interface ICommand : IRequest { }

    public interface ICommand<TResponse> : IRequest<TResponse> { }
}
