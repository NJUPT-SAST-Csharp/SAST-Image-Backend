namespace SastImg.Application.EventBus
{
    public interface IExternalEventBus
    {
        public Task PublishEventAsync(string channel, string message);

        public Task ListenEventAsync(string channel, Action<string?> e);
    }
}
