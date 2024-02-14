using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateProfile
{
    internal sealed class UpdateProfileCommandHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<UpdateProfileCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(request.Requester.Id, cancellationToken);
            user.UpdateProfile(request.Nickname, request.Biography);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
