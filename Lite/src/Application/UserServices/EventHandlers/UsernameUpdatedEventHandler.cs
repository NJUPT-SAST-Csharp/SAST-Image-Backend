using Domain.Core.Event;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.UserEntity;

namespace Application.UserServices.EventHandlers;

internal sealed class UsernameUpdatedEventHandler(IUserModelRepository repository)
    : IDomainEventHandler<UsernameResetEvent>
{
    public async ValueTask Handle(UsernameResetEvent e, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(e.UserId, cancellationToken);

        user.ResetUsername(e);
    }
}
