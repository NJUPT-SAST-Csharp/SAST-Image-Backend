using Domain.Shared;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record class UpdateAvatarCommand(IImageFile Avatar, Actor Actor) : ICommand { }

internal sealed class UpdateAvatarCommandHandler(IUserRepository repository)
    : ICommandHandler<UpdateAvatarCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateAvatarCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.UpdateAvatar(command);

        return Unit.Value;
    }
}
