using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.AddUser
{
    internal sealed class AddUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork)
        : ICommandRequestHandler<AddUserCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.CreateNewUser(request.UserId);
            await _repository.AddNewUserAsync(user, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
