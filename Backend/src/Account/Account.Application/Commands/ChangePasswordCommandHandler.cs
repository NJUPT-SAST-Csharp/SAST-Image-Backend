using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.Services;
using Mediator;

namespace Account.Application.Commands;

public sealed class ChangePasswordCommandHandler(
    IUserRepository repository,
    IPasswordGenerator generator
) : ICommandHandler<ChangePasswordCommand>
{
    public async ValueTask<Unit> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetByIdAsync(request.Requester.Id, cancellationToken);

        await user.ResetPasswordAsync(request, generator);

        return Unit.Value;
    }
}
