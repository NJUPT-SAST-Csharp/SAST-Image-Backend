using Domain.Shared;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Domain.UserAggregate.Commands;

public sealed record UpdateBiographyCommand(Biography Biography, Actor Actor) : ICommand { }

internal sealed class UpdateBiographyCommandHandler(IUserRepository repository)
    : ICommandHandler<UpdateBiographyCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateBiographyCommand e,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetAsync(e.Actor.Id, cancellationToken);

        user.UpdateBiography(e);

        return Unit.Value;
    }
}
