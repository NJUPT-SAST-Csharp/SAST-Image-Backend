using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateHeader
{
    internal sealed class UpdateHeaderCommandHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork,
        IHeaderStorageClient client
    ) : ICommandRequestHandler<UpdateHeaderCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHeaderStorageClient _client = client;

        public async Task Handle(UpdateHeaderCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(request.Requester.Id, cancellationToken);

            var url = await _client.UploadHeaderAsync(
                request.Requester.Id,
                request.HeaderFile,
                cancellationToken
            );

            user.UpdateHeader(url);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
