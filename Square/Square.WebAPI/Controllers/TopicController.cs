using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Square.Application.TopicServices.CreateTopic;
using Square.Application.TopicServices.DeleteTopic;
using Square.Application.TopicServices.UpdateTopicInfo;
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
            new CreateTopicCommand(
                request.Title,
                request.Description,
                request.MainColumnText,
                request.Images,
                User
            ),
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
