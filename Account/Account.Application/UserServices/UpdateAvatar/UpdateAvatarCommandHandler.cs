using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.UserServices.UpdateAvatar
{
    internal sealed class UpdateAvatarCommandHandler(
        IUserRepository repository,
        IAvatarStorageRepository storage,
        IUnitOfWork unit
    ) : ICommandRequestHandler<UpdateAvatarCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IAvatarStorageRepository _storage = storage;
        private readonly IUnitOfWork _unit = unit;

        public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

            if (request.Avatar is not null)
            {
                var url = await _storage
                    .UploadAvatarAsync(user.Id, request.Avatar)
                    .WaitAsync(cancellationToken);

                user.UpdateAvatar(url);
            }
            else
            {
                user.UpdateAvatar(null);
            }

            await _unit.CommitChangesAsync(cancellationToken);
        }
    }
}
