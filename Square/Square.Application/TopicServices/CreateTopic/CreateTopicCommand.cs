using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Square.Domain;

namespace Square.Application.TopicServices.CreateTopic
{
    public sealed class CreateTopicCommand(
        string title,
        string description,
        string mainColumnText,
        IFormFileCollection images,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public string Title { get; } = title;

        public string Description { get; } = description;

        public string MainColumnText { get; } = mainColumnText;

        public IFormFileCollection Images { get; } = images;

        public RequesterInfo Requester { get; } = new(user);
    }
}
