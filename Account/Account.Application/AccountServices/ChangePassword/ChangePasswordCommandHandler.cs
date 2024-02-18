using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordCommandHandler(IUserRepository repository, IUnitOfWork unit)
        : ICommandRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unit = unit;

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

            await user.ResetPasswordAsync(request.NewPassword).WaitAsync(cancellationToken);

            await _unit.CommitChangesAsync(cancellationToken);
        }
    }
}
