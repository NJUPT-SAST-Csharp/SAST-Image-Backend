﻿namespace Primitives.Message
{
    public interface IMessagePublisher
    {
        /// <summary>
        /// Message will be sent to MQ ( auto deserialized ) once this method is called.
        /// </summary>
        /// <param name="message">Message to be sent to MQ.</param>
        public Task<bool> PublishAsync<TMessage>(
            TMessage message,
            CancellationToken cancellationToken = default
        )
            where TMessage : IMessage;
    }
}
