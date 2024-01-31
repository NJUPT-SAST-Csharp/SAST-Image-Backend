namespace Primitives.Command
{
    public interface ICommandRequestSender
    {
        public Task<TResponse> CommandAsync<TResponse>(
            ICommandRequest<TResponse> command,
            CancellationToken cancellationToken = default
        );

        public Task CommandAsync(
            ICommandRequest command,
            CancellationToken cancellationToken = default
        );
    }
}
