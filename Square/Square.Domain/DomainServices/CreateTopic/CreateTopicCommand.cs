using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.DomainServices.CreateTopic
{
    public sealed class CreateTopicCommand(string title, string description, ClaimsPrincipal user)
        : ICommandRequest<Result<TopicId>>
    {
        public TopicTitle Title { get; } = new(title);

        public TopicDescription Description { get; } = new(description);

        public RequesterInfo Requester { get; } = new(user);
    }
}
