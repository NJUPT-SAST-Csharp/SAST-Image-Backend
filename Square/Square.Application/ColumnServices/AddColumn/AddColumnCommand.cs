using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Square.Application.SeedWorks;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices.AddColumn
{
    public sealed class AddColumnCommand(
        long topicId,
        string? text,
        IFormFileCollection images,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public TopicId TopicId { get; } = new(topicId);
        public string? Text { get; } = text;
        public IFormFileCollection Images { get; } = images;

        public RequesterInfo Requester { get; } = new(user);
    }
}
