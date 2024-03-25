using Exceptions.Exceptions;
using Primitives;
using Primitives.Command;
using Square.Domain.TopicAggregate;

namespace Square.Application.TopicServices.UpdateTopicInfo
{
    internal sealed class UpdateTopicInfoCommandHandler(
        ITopicRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<UpdateTopicInfoCommand>
    {
        private readonly ITopicRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            UpdateTopicInfoCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository.GetTopicAsync(request.TopicId, cancellationToken);

            if (request.Requester.Id != topic.AuthorId && request.Requester.IsAdmin == false)
            {
                throw new NoPermissionException();
            }

            topic.UpdateInfo(request.Title, request.Description);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
