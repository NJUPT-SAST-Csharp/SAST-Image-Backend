using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.Commands.UnsubscribeTopic
{
    internal sealed class UnsubscribeTopicCommandHandler(ITopicRepository repository)
        : ICommandRequestHandler<UnsubscribeTopicCommand, Result>
    {
        private readonly ITopicRepository _repository = repository;

        public async Task<Result> Handle(
            UnsubscribeTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository.GetTopicAsync(request.TopicId);

            if (topic is null)
            {
                return Result.Fail(Error.NotFound(topic));
            }

            topic.Unsubscribe(request);

            return Result.Success;
        }
    }
}
