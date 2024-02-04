using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.AddUser
{
    internal sealed class AddUserCommandHandler(IUserRepository repository)
        : ICommandRequestHandler<AddUserCommand>
    {
        private readonly IUserRepository _repository = repository;

        public Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.CreateNewUser(request.UserId);
            return _repository.AddNewUserAsync(user, cancellationToken);
        }
    }
}
