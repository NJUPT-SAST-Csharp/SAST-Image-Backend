using Primitives;
using Primitives.Command;
using Square.Application.TopicServices;
using Square.Domain.TopicAggregate;

namespace Square.Application.ColumnServices.DeleteColumn
{
    internal sealed class DeleteColumnCommandHandler(
        ITopicRepository repository,
        ITopicImageStorage storage,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<DeleteColumnCommand>
    {
        private readonly ITopicRepository _repository = repository;
        private readonly ITopicImageStorage _storage = storage;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
        {
            var topic = await _repository.GetTopicAsync(request.TopicId, cancellationToken);

            var column = topic.Columns.FirstOrDefault(c => c.Id == request.ColumnId);

            if (column is null)
                return;

            await _storage.DeleteImagesAsync(column.Images, cancellationToken);

            topic.DeleteColumn(request.ColumnId);

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
