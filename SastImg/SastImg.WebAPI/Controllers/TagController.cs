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
    /// TODO: complete
    /// </summary>
    [Route("api/sastimg")]
    [ApiController]
    public class TagController(IQueryRequestSender querySender, ICommandRequestSender commandSender)
        : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(nameof(AuthorizationRole.Admin))]
        [HttpGet("tags/all")]
        public async Task<Ok<IEnumerable<TagDto>>> GetAllTags(CancellationToken cancellationToken)
        {
            var tags = await _querySender.QueryAsync(new GetAllTagsQuery(), cancellationToken);
            return Responses.Data(tags);
        }

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("tags/search")]
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
        /// TODO: complete
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("tags")]
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
        /// TODO: complete
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("tag")]
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
