using FoxResult;
using Primitives;
using Primitives.Command;
using Square.Domain.ColumnAggregate;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.DomainServices.CreateTopic
{
    internal sealed class CreateTopicCommandHandler(
        ITopicRepository topics,
        IColumnRepository columns,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<CreateTopicCommand, Result<TopicId>>
    {
        private readonly ITopicRepository _topics = topics;
        private readonly IColumnRepository _columns = columns;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<TopicId>> Handle(
            CreateTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = Topic.CreateNewTopic(request);

            await _topics.AddTopicAsync(topic);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Return(topic.Id);
        }
    }
}
