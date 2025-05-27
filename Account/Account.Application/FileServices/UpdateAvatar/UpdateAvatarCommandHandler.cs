using Account.Domain.UserEntity.Services;
using Mediator;
using Primitives;

namespace Account.Application.FileServices.UpdateAvatar;

public sealed class UpdateAvatarCommandHandler(
    IUserRepository repository,
    IAvatarStorageRepository storage,
    IUnitOfWork unit
) : ICommandHandler<UpdateAvatarCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateAvatarCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

        if (request.Avatar is not null)
        {
            var url = await storage.UploadAvatarAsync(user.Id, request.Avatar, cancellationToken);

            user.UpdateAvatar(url);
        }
        else
        {
            user.UpdateAvatar(null);
        }

        await unit.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
