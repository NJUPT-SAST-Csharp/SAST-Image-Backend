using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.CreateTopic
{
    internal sealed class CreateTopicCommandHandler(
        ITopicRepository topics,
        ITopicUniquenessChecker checker
    ) : ICommandRequestHandler<CreateTopicCommand, Result<TopicId>>
    {
        private readonly ITopicRepository _topics = topics;
        private readonly ITopicUniquenessChecker _checker = checker;

        public async Task<Result<TopicId>> Handle(
            CreateTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine(request.CategoryId);
            Console.WriteLine(request.Description);
            Console.WriteLine(request.Title);

            var result = await Topic
                .CreateNewTopicAsync(request, _checker, _topics)
                .WaitAsync(cancellationToken);

            if (result.IsFailure)
            {
                return Result.From<TopicId>(result);
            }

            return Result.Return(result.Value.Id);
        }
    }
}
