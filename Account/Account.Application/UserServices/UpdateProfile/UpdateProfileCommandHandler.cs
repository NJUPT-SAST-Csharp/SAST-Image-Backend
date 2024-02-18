using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.UserServices.UpdateProfile
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
            var user = await _repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

            user.UpdateProfile(
                request.Nickname,
                request.Biography,
                request.Birthday,
                request.Website
            );

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
