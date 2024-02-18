using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeCommandHandler(IUserRepository users, IUnitOfWork unit)
        : ICommandRequestHandler<AuthorizeCommand>
    {
        private readonly IUserRepository _users = users;
        private readonly IUnitOfWork _unit = unit;

        public async Task Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetUserByIdAsync(request.UserId, cancellationToken);

            user.UpdateAuthorizations(request.Roles);

            await _unit.CommitChangesAsync(cancellationToken);
        }
    }
}
