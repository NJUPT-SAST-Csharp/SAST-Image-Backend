using Account.Domain.UserEntity.Services;
using Mediator;
using Primitives;

namespace Account.Application.FileServices.UpdateHeader;

public sealed class UpdateHeaderCommandHandler(
    IUserRepository repository,
    IHeaderStorageRepository storage,
    IUnitOfWork unit
) : ICommandHandler<UpdateHeaderCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateHeaderCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

        if (request.Header is not null)
        {
            var url = await storage.UploadHeaderAsync(user.Id, request.Header, cancellationToken);

            user.UpdateHeader(url);
        }
        else
        {
            user.UpdateHeader(null);
        }

        await unit.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
