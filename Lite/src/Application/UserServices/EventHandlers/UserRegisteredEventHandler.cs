using Domain.Core.Event;
using Domain.UserAggregate.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class UserRegisteredEventHandler(IUserModelRepository repository)
    : IDomainEventHandler<UserRegisteredEvent>
{
    public async ValueTask Handle(UserRegisteredEvent e, CancellationToken cancellationToken)
    {
        UserModel user = new(e);

        await repository.AddAsync(user, cancellationToken);
    }
}
