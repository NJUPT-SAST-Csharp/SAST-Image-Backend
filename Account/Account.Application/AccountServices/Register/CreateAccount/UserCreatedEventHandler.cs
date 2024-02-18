using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Domain.UserEntity.Events;
using Messenger;
using Primitives.DomainEvent;

namespace Account.Application.AccountServices.Register.CreateAccount
{
    internal class UserCreatedEventHandler(IMessagePublisher publisher)
        : IDomainEventHandler<UserCreatedEvent>
    {
        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await publisher.PublishAsync(
                "account.user.created",
                new UserCreatedMessage(notification.UserId),
                cancellationToken
            );
        }
    }
}
