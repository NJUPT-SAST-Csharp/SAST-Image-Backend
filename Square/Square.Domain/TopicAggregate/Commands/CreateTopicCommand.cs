using System.Security.Claims;
using FoxResult;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands
{
    public sealed class CreateTopicCommand(
        string title,
        string description,
        string mainColumnText,
        IFormFileCollection images,
        ClaimsPrincipal user
    ) : ICommandRequest<Result<TopicId>>
    {
        public TopicTitle Title { get; } = new(title);

        public TopicDescription Description { get; } = new(description);

        public TopicColumnText MainColumnText { get; } = new(mainColumnText);

        public IFormFileCollection Images { get; } = images;

        public RequesterInfo Requester { get; } = new(user);
    }
}
