using System.ComponentModel.DataAnnotations;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.TagServices;
using SastImg.Application.TagServices.CreateTag;
using SastImg.Application.TagServices.GetAllTags;
using SastImg.Application.TagServices.SearchTags;
using SastImg.Domain.AlbumTagEntity;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers;

/// <summary>
/// Controller for tag related operations.
/// </summary>
[ApiController]
[Route("api/sastimg")]
[Produces("application/json")]
public class TagController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get All Tags
    /// </summary>
    /// <remarks>
    /// Get all tags
    /// <para>ADMIN authorization is required</para>
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The tags</response>
    [Authorize(nameof(Role.ADMIN))]
    [HttpGet("tags/all")]
    [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<TagDto>>> GetAllTags(CancellationToken cancellationToken)
    {
        var tags = await mediator.Send(new GetAllTagsQuery(), cancellationToken);
        return Responses.Data(tags);
    }

    /// <summary>
    /// Search Tags
    /// </summary>
    /// <remarks>
    /// Search tags by name
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="name">The tag name(Support approximate search)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">The tags</response>
    [Authorize]
    [HttpGet("tags/search")]
    [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
    public async Task<Ok<IEnumerable<TagDto>>> SearchTags(
        [MaxLength(TagName.MaxLength)] string name,
        CancellationToken cancellationToken
    )
    {
        var tagsDto = await mediator.Send(new SearchTagsQuery(name), cancellationToken);
        return Responses.Data(tagsDto);
    }

    public readonly record struct CreateTagRequest(TagName Name);

    /// <summary>
    /// Create ImageTag
    /// </summary>
    /// <remarks>
    /// Create a new tag.
    /// <para>Authorization is required</para>
    /// </remarks>
    /// <param name="request">The new tag info</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="201">The tag is created successfully</response>
    [Authorize]
    [HttpPost("tag")]
    [ProducesResponseType<TagDto>(StatusCodes.Status201Created)]
    public async Task<Created<TagDto>> CreateTag(
        [FromBody] CreateTagRequest request,
        CancellationToken cancellationToken
    )
    {
        var tagDto = await mediator.Send(new CreateTagCommand(request.Name), cancellationToken);
        return Responses.Created(tagDto);
    }
}
