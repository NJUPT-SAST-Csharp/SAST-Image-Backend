using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.CreateTopic
{
    public sealed class CreateTopicCommand(
        string title,
        string description,
        int categoryId,
        ClaimsPrincipal user
    ) : ICommandRequest<Result<TopicId>>
    {
        public TopicTitle Title { get; } = new(title);

        public TopicDescription Description { get; } = new(description);

        public CategoryId CategoryId { get; } = new(categoryId);

        public RequesterInfo Requester { get; } = new(user);
    }
}
