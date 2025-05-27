using Account.Domain.UserEntity.Services;
using Mediator;
using Primitives;

namespace Account.Application.UserServices.UpdateProfile;

public sealed class UpdateProfileCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProfileCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

        user.UpdateProfile(request.Nickname, request.Biography, request.Birthday, request.Website);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
