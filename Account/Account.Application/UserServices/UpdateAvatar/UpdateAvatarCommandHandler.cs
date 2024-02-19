using Account.Domain.UserEntity.Services;
using Primitives.Command;

namespace Account.Application.UserServices.UpdateAvatar
{
    internal sealed class UpdateAvatarCommandHandler(IUserRepository repository)
        : ICommandRequestHandler<UpdateAvatarCommand>
    {
        private readonly IUserRepository _repository = repository;

        public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);
        }
    }
}
