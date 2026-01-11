using Domain.Core.Event;
using Domain.UserAggregate.Events;
using Domain.UserAggregate.UserEntity;

namespace Application.UserServices.EventHandlers;

internal sealed class NicknameUpdatedEventHandler(IUserModelRepository repository)
    : IDomainEventHandler<NicknameUpdatedEvent>
{
    public async ValueTask Handle(NicknameUpdatedEvent e, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(e.Id, cancellationToken);

        user.UpdateNickname(e.Nickname);
    }
}
