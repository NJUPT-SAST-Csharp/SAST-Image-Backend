using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Square.Domain.TopicAggregate.Commands.CreateTopic;
using Square.Domain.TopicAggregate.Commands.DeleteTopic;
using Square.Domain.TopicAggregate.Commands.UpdateTopicInfo;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers;

[ApiController]
[Route("api/square")]
public class TopicController(ICommandRequestSender commandSender) : ControllerBase
{
    private readonly ICommandRequestSender _commandSender = commandSender;

    [Authorize]
    [HttpPost("topic")]
    [DisableRequestSizeLimit]
    public Task CreateTopic(
        [FromForm] CreateTopicRequest request,
        CancellationToken cancellationToken = default
    )
    {
        return _commandSender.CommandAsync(
            new CreateTopicCommand(request.Title, request.Description, User),
            cancellationToken
        );
    }

    [Authorize]
    [HttpDelete("topic/{topicId}")]
    public Task DeleteTopic([FromRoute] long topicId, CancellationToken cancellationToken = default)
    {
        return _commandSender.CommandAsync(
            new DeleteTopicCommand(topicId, User),
            cancellationToken
        );
    }

    [Authorize]
    [HttpPut("topic/{topicId}")]
    public Task UpdateTopicInfo(
        [FromRoute] long topicId,
        [FromBody] UpdateTopicInfoRequest request,
        CancellationToken cancellationToken = default
    )
    {
        return _commandSender.CommandAsync(
            new UpdateTopicInfoCommand(topicId, request.Title, request.Description, User),
            cancellationToken
        );
    }
}
