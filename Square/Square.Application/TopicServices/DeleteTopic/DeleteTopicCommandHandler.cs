using Exceptions.Exceptions;
using Primitives;
using Primitives.Command;
using Square.Domain.TopicAggregate;

namespace Square.Application.TopicServices.DeleteTopic
{
    internal class DeleteTopicCommandHandler(
        ITopicRepository repository,
        ITopicImageStorage storage,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<DeleteTopicCommand>
    {
        private readonly ITopicRepository _repository = repository;
        private readonly ITopicImageStorage _storage = storage;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _repository.GetTopicAsync(request.TopicId, cancellationToken);

            if (topic.AuthorId != request.Requester.Id && request.Requester.IsAdmin == false)
            {
                throw new NoPermissionException();
            }

            var repositoryDeleteTask = _repository.DeleteTopicAsync(topic, cancellationToken);

            var storageDeleteTask = _storage.DeleteImagesAsync(
                topic.Columns.SelectMany(c => c.Images),
                cancellationToken
            );

            await Task.WhenAll(storageDeleteTask, repositoryDeleteTask);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
