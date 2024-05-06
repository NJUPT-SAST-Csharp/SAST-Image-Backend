using MediatR;

namespace Primitives.Command
{
    public interface IBaseCommand { }

    public interface ICommandRequest : IRequest, IBaseCommand { }

    public interface ICommandRequest<TResponse> : IRequest<TResponse>, IBaseCommand { }
}
