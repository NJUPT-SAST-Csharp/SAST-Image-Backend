using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.DeleteTopic
{
    internal sealed class DeleteTopicCommandHandler(ITopicRepository repository)
        : ICommandRequestHandler<DeleteTopicCommand, Result>
    {
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

            var result = topic.DeleteTopic(request, _repository);

            if (result.IsFailure)
            {
                return result;
            }

            return Result.Success;
        }
    }
}
