using FoxResult.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using Square.Application.TopicServices.Queries.GetTopic;
using Square.Application.TopicServices.Queries.GetTopics;
using Square.Domain.TopicAggregate.Commands.CreateTopic;
using Square.Domain.TopicAggregate.Commands.DeleteTopic;
using Square.Domain.TopicAggregate.Commands.UpdateTopicInfo;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers;

[ApiController]
[Route("api/square")]
public class TopicController(ICommandRequestSender commandSender, IQueryRequestSender querySender)
    : ControllerBase
{
    private readonly ICommandRequestSender _commandSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    /// <summary>
    /// Create topic.
    /// </summary>
    /// <remarks>
    /// Create a new topic.
    /// </remarks>
    /// <param name="request">New topic info</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">Topic created successfully</response>
    /// <response code="409">Topic title already exists</response>
    [Authorize]
    [HttpPost("topic")]
    [ProducesResponseType<DataResponseType<TopicId>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IResult> CreateTopic(
        [FromBody] CreateTopicRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = _commandSender.CommandAsync(
            new CreateTopicCommand(request.Title, request.Description, request.CategoryId, User),
            cancellationToken
        );

        return Results.Extensions.FromTask(result);
    }

    /// <summary>
    /// Delete topic.
    /// </summary>
    /// <remarks>
    /// Delete a topic permanently.
    /// </remarks>
    /// <param name="topicId">Id of the topic to be deleted.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">Topic deleted successfully</response>
    /// <response code="404">Topic not found</response>
    [Authorize]
    [HttpDelete("topic/{topicId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IResult> DeleteTopic(
        [FromRoute] long topicId,
        CancellationToken cancellationToken = default
    )
    {
        var result = _commandSender.CommandAsync(
            new DeleteTopicCommand(topicId, User),
            cancellationToken
        );

        return Results.Extensions.FromTask(result);
    }

    /// <summary>
    /// Update topic info.
    /// </summary>
    /// <param name="topicId">Id of the topic to be updated</param>
    /// <param name="request">The new topic info.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The topic info was updated successfully</response>
    /// <response code="404">Topic not found</response>
    /// <response code="409">The topic title already exists</response>
    [Authorize]
    [HttpPut("topic/{topicId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IResult> UpdateTopicInfo(
        [FromRoute] long topicId,
        [FromBody] UpdateTopicInfoRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = _commandSender.CommandAsync(
            new UpdateTopicInfoCommand(topicId, request.Title, request.Description, User),
            cancellationToken
        );

        return Results.Extensions.FromTask(result);
    }

    /// <summary>
    /// Get topics.
    /// </summary>
    /// <remarks>
    /// Get global topics. Order by update time default.
    /// </remarks>
    /// <param name="category">Category Id, all topics if empty.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The topics.</response>
    [HttpGet("topics")]
    [ProducesResponseType<DataResponseType<IEnumerable<TopicDto>>>(StatusCodes.Status200OK)]
    public Task<IResult> GetTopics(
        [FromQuery] int? category = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = _querySender.QueryAsync(new GetTopicsQuery(category), cancellationToken);

        return Results.Extensions.FromTask(result);
    }

    /// <summary>
    /// Get topic.
    /// </summary>
    /// <remarks>
    /// Get a topic with its detail info.
    /// </remarks>
    /// <param name="topicId">Id of the topic</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("topic/{topicId}")]
    [ProducesResponseType<DataResponseType<TopicDetailedDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IResult> GetTopic(
        [FromRoute] long topicId,
        CancellationToken cancellationToken = default
    )
    {
        var result = _querySender.QueryAsync(new GetTopicQuery(topicId), cancellationToken);

        return Results.Extensions.FromTask(result);
    }
}
