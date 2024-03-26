using Primitives;
using Primitives.Command;
using Square.Application.TopicServices;
using Square.Domain.TopicAggregate;

namespace Square.Application.ColumnServices.AddColumn
{
    internal sealed class AddColumnCommandHandler(
        ITopicRepository repository,
        IUnitOfWork unitOfWork,
        ITopicImageStorage storage
    ) : ICommandRequestHandler<AddColumnCommand>
    {
        private readonly ITopicRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITopicImageStorage _storage = storage;

        public async Task Handle(AddColumnCommand request, CancellationToken cancellationToken)
        {
            var topic = await _repository.GetTopicAsync(request.TopicId, cancellationToken);

            var images = await _storage.UploadImagesAsync(request.Images, cancellationToken);

            topic.AddColumn(request.Requester.Id, request.Text, images);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
