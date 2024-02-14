using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateAvatar
{
    internal sealed class UpdateAvatarCommandHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<UpdateAvatarCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(request.Requester.Id, cancellationToken);

            // TODO: Implement the logic to update the user's avatar

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
