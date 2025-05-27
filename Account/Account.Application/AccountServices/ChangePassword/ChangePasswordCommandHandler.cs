using Account.Domain.UserEntity.Services;
using Mediator;
using Primitives;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword;

public sealed class ChangePasswordCommandHandler(IUserRepository repository, IUnitOfWork unit)
    : ICommandHandler<ChangePasswordCommand>
{
    public async ValueTask<Unit> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

        user.ResetPassword(request.NewPassword);

        await unit.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
