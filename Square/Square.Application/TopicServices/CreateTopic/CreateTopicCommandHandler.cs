using Primitives;
using Primitives.Command;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.CreateTopic
{
    internal sealed class CreateTopicCommandHandler(
        ITopicImageStorage storage,
        ITopicRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicImageStorage _storage = storage;
        private readonly ITopicRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var images = await _storage.UploadImagesAsync(request.Images, cancellationToken);

            var topic = Topic.CreateNewTopic(
                request.Requester.Id,
                request.Title,
                request.Description
            );

            topic.AddColumn(request.Requester.Id, request.MainColumnText, images);

            await _repository.AddTopicAsync(topic, cancellationToken);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
