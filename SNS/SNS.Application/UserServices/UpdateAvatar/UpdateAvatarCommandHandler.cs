using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateAvatar
{
    internal sealed class UpdateAvatarCommandHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork,
        IAvatarStorageClient client
    ) : ICommandRequestHandler<UpdateAvatarCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAvatarStorageClient _client = client;

        public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(request.Requester.Id, cancellationToken);

            var url = await _client.UploadAvatarAsync(
                request.Requester.Id,
                request.AvatarFile,
                cancellationToken
            );

            user.UpdateAvatar(url);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
