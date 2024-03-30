using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.Commands.SubscribeTopic
{
    public sealed class SubscribeTopicCommandHandler(ITopicRepository repository)
        : ICommandRequestHandler<SubscribeTopicCommand, Result>
    {
        private readonly ITopicRepository _repository = repository;

        public async Task<Result> Handle(
            SubscribeTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository.GetTopicAsync(request.TopicId);

            if (topic is null)
            {
                return Result.Fail(Error.NotFound(topic));
            }

            topic.Subscribe(request);

            return Result.Success;
        }
    }
}
