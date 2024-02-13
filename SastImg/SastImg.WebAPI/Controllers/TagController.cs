using System.ComponentModel.DataAnnotations;
using Auth.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.TagServices;
using SastImg.Application.TagServices.CreateTag;
using SastImg.Application.TagServices.GetAllTags;
using SastImg.Application.TagServices.GetTags;
using SastImg.Application.TagServices.SearchTags;
using SastImg.WebAPI.Requests.TagRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// Controller for tag related operations.
    /// </summary>
    [ApiController]
    [Route("api/sastimg")]
    [Produces("application/json")]
    public class TagController(IQueryRequestSender querySender, ICommandRequestSender commandSender)
        : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// Get All Tags
        /// </summary>
        /// <remarks>
        /// <para>Get all tags</para>
        /// <para>Admin authorization is required</para>
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The tags</response>
        [Authorize(nameof(AuthorizationRole.Admin))]
        [HttpGet("tags/all")]
        [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<TagDto>>> GetAllTags(CancellationToken cancellationToken)
        {
            var tags = await _querySender.QueryAsync(new GetAllTagsQuery(), cancellationToken);
            return Responses.Data(tags);
        }

        /// <summary>
        /// Search Tags
        /// </summary>
        /// <remarks>
        /// <para>Search tags by name</para>
        /// <para>Authorization is required</para>
        /// </remarks>
        /// <param name="name">The tag name(Support approximate search)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The tags</response>
        [Authorize]
        [HttpGet("tags/search")]
        [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<TagDto>>> SearchTags(
            [MaxLength(10)] string name,
            CancellationToken cancellationToken
        )
        {
            var tagsDto = await _querySender.QueryAsync(
                new SearchTagsQuery(name),
                cancellationToken
            );
            return Responses.Data(tagsDto);
        }

        /// <summary>
        /// Get Tags by Ids
        /// </summary>
        /// <remarks>
        /// <para>
        /// Get tags by ids <br/>
        /// This endpoint is used to get tag's info when displaying images.
        /// </para>
        /// <para>Authorization is required</para>
        /// </remarks>
        /// <param name="tagIds">The tags' ids (up to 5)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The tags</response>
        [Authorize]
        [HttpGet("tags")]
        [ProducesResponseType<IEnumerable<TagDto>>(StatusCodes.Status200OK)]
        public async Task<Ok<IEnumerable<TagDto>>> GetTags(
            [FromQuery] long[] tagIds,
            CancellationToken cancellationToken
        )
        {
            var tagsDto = await _querySender.QueryAsync(
                new GetTagsQuery(tagIds),
                cancellationToken
            );
            return Responses.Data(tagsDto);
        }

        /// <summary>
        /// Create Tag
        /// </summary>
        /// <remarks>
        /// <para>Create a new tag</para>
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
            var tagDto = await _commandSender.CommandAsync(
                new CreateTagCommand(request.Name),
                cancellationToken
            );
            return Responses.Created(tagDto);
        }
    }
}
