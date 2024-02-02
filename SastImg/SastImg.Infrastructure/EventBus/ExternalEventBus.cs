using DotNetCore.CAP;
using Messenger;

namespace SastImg.Infrastructure.EventBus
{
    internal sealed class ExternalEventBus(ICapPublisher publisher) : IMessagePublisher
    {
        private readonly ICapPublisher _publisher = publisher;

        public async Task<bool> PublishAsync<TMessage>(
            string channel,
            TMessage message,
            CancellationToken cancellationToken = default
        )
            where TMessage : IMessage
        {
            await _publisher.PublishAsync(channel, message, cancellationToken: cancellationToken);
            return true;
        }
    }
}
