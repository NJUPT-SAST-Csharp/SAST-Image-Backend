using System.Security.Claims;
using FoxResult;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.TopicAggregate.TopicEntity.Commands.AddTopicColumn
{
    public sealed class AddTopicColumnCommand(
        long topicId,
        string? text,
        IFormFileCollection images,
        ClaimsPrincipal user
    ) : ICommandRequest<Result<ColumnId>>
    {
        public TopicId TopicId { get; } = new(topicId);
        public TopicColumnText Text { get; } = new(text);
        public IFormFileCollection Images { get; } = images;
        public RequesterInfo Requester { get; } = new(user);
    }
}
