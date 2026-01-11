using Domain.Core.Event;
using Domain.UserAggregate.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class BiographyUpdatedEventHandler(IUserModelRepository repository)
    : IDomainEventHandler<BiographyUpdatedEvent>
{
    public async ValueTask Handle(BiographyUpdatedEvent e, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(e.User, cancellationToken);

        user.UpdateBiography(e);
    }
}
