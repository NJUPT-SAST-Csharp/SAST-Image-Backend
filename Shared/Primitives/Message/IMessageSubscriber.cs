using DotNetCore.CAP;

namespace Primitives.Message
{
    public interface IMessageSubscriber<TMessage> : ICapSubscribe
        where TMessage : IMessage
    {
        /// <summary>
        /// Message will be received and handled ( auto deserialized ) once MQ contains a message.
        /// </summary>
        /// <remarks>
        /// This method should always be marked with <see cref="SubscribeAttribute"/>
        /// </remarks>
        /// <param name="message">Message to be received from MQ.</param>
        public Task SubscribeAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
