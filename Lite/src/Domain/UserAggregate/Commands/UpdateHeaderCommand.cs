using Domain.Shared;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record UpdateHeaderCommand(IImageFile Header, Actor Actor) : ICommand { }

internal sealed class UpdateHeaderCommandHandler(IUserRepository repository)
    : ICommandHandler<UpdateHeaderCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateHeaderCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.UpdateHeader(command);

        return Unit.Value;
    }
}
