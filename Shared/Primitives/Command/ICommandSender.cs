namespace Primitives.Command
{
    public interface ICommandSender
    {
        public Task<TResponse> SendCommandAsync<TCommand, TResponse>(
            TCommand command,
            CancellationToken cancellationToken = default
        )
            where TCommand : ICommand<TResponse>;
    }
}
