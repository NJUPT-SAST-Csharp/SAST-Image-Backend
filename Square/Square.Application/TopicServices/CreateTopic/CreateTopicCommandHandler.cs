using Primitives;
using Primitives.Command;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.CreateTopic
{
    internal sealed class CreateTopicCommandHandler(
        ITopicImageStorageRepository storage,
        ITopicRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicImageStorageRepository _storage = storage;
        private readonly ITopicRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var result = request.Images.Select(
                image => _storage.UploadImageAsync(image, cancellationToken)
            );

            var imageUrls = await Task.WhenAll(result);

            var images = Array.ConvertAll(
                imageUrls,
                urls => new TopicImage(urls.Item1, urls.Item2)
            );

            var topic = Topic.CreateNewTopic(
                request.Requester.Id,
                request.Title,
                request.Description
            );

            topic.AddColumn(request.Requester.Id, request.MainColumnText, images);

            await _repository.AddTopicAsync(topic);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
