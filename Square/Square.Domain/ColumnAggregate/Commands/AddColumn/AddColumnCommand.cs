using System.Security.Claims;
using FoxResult;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.ColumnAggregate.Commands.AddColumn
{
    public sealed class AddColumnCommand(
        long topicId,
        string? text,
        IFormFileCollection images,
        ClaimsPrincipal user
    ) : ICommandRequest<Result<ColumnId>>
    {
        public TopicId TopicId { get; } = new(topicId);
        public ColumnText Text { get; } = new(text);
        public IFormFileCollection Images { get; } = images;
        public RequesterInfo Requester { get; } = new(user);
    }
}
