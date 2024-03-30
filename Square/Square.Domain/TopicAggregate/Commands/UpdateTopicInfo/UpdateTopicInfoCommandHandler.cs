using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.Commands.UpdateTopicInfo
{
    public sealed class UpdateTopicInfoCommandHandler(
        ITopicRepository repository,
        ITopicUniquenessChecker checker
    ) : ICommandRequestHandler<UpdateTopicInfoCommand, Result>
    {
        private readonly ITopicRepository _repository = repository;
        private readonly ITopicUniquenessChecker _checker = checker;

        public async Task<Result> Handle(
            UpdateTopicInfoCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository.GetTopicAsync(request.TopicId);

            if (topic is null)
            {
                return Result.Fail(Error.NotFound(topic));
            }

            var result = await topic.UpdateTopicInfoAsync(request, _checker);

            if (result.IsFailure)
            {
                return result;
            }

            return Result.Success;
        }
    }
}
