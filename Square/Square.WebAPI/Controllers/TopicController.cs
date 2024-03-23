using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Square.Application.TopicServices.CreateTopic;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers;

[ApiController]
[Route("api/square")]
public class TopicController(ICommandRequestSender sender) : ControllerBase
{
    private readonly ICommandRequestSender _sender = sender;

    [HttpPost("topic")]
    [DisableRequestSizeLimit]
    public async Task CreateTopicAsync(
        [FromForm] CreateTopicRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await _sender.CommandAsync(
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
}
