using FoxResult;
using Primitives;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.DeleteTopic
{
    internal sealed class DeleteTopicCommandHandler(
        IUnitOfWork unitOfWork,
        ITopicRepository repository
    ) : ICommandRequestHandler<DeleteTopicCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITopicRepository _repository = repository;

        public async Task<Result> Handle(
            DeleteTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository.GetTopicAsync(request.TopicId);

            if (topic is null)
            {
                return Result.Fail(Error.NotFound<Topic>());
            }

            if (topic.IsManagedBy(request.Requester) == false)
            {
                return Result.Fail(Error.Forbidden);
            }

            await _repository.DeleteTopicAsync(topic);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
