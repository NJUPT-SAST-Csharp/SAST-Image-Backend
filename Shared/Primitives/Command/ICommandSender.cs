namespace Primitives.Command
{
    public interface ICommandSender
    {
        public Task<TResponse> CommandAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default
        );

        public Task CommandAsync(ICommand command, CancellationToken cancellationToken = default);
    }
}
