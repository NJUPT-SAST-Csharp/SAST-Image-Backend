using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Domain.UserEntity.Events;
using Mediator;
using Messenger;

namespace Account.Application.AccountServices.Register.CreateAccount;

public sealed class UserCreatedEventHandler(IMessagePublisher publisher)
    : IDomainEventHandler<UserCreatedEvent>
{
    public async ValueTask Handle(
        UserCreatedEvent notification,
        CancellationToken cancellationToken
    )
    {
        await publisher.PublishAsync(
            "account.user.created",
            new UserCreatedMessage(notification.UserId),
            cancellationToken
        );
    }
}
