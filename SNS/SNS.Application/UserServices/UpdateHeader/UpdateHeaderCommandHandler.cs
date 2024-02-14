using Primitives;
using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateHeader
{
    internal sealed class UpdateHeaderCommandHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<UpdateHeaderCommand>
    {
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateHeaderCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserAsync(request.Requester.Id, cancellationToken);

            // TODO: Implement the logic to update the user's header

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
