using SastImg.Application.EventBus;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Event
{
    internal class ExternalEventBus : IExternalEventBus
    {
        private readonly ISubscriber _sub;

        public ExternalEventBus(IConnectionMultiplexer connectionMultiplexer)
        {
            _sub = connectionMultiplexer.GetSubscriber();
        }

        public Task ListenEventAsync(string channel, Action<string?> e)
        {
            return _sub.SubscribeAsync(
                RedisChannel.Literal(nameof(SastImg) + channel),
                (c, m) => e(m)
            );
        }

        public Task PublishEventAsync(string channel, string message)
        {
            return _sub.PublishAsync(
                RedisChannel.Literal(nameof(SastImg) + channel),
                message,
                CommandFlags.FireAndForget
            );
        }
    }
}
