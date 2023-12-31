using System.ComponentModel.DataAnnotations;
using Auth.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Request;
using SastImg.Application.TagServices;
using SastImg.Application.TagServices.GetAllTags;
using SastImg.Application.TagServices.GetTags;
using SastImg.Application.TagServices.SearchTags;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api")]
    [ApiController]
    public class TagController(IQueryRequestSender querySender) : ControllerBase
    {
        private readonly IQueryRequestSender _querySender = querySender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(nameof(AuthorizationRole.Admin))]
        [HttpGet("tags/all")]
        public async Task<Ok<IEnumerable<TagDto>>> GetAllTags(CancellationToken cancellationToken)
        {
            var tags = await _querySender.QueryAsync(
                new GetAllTagsQueryRequest(),
                cancellationToken
            );
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
                new SearchTagsQueryRequest(name),
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
            [MaxLength(5)] [Range(0, long.MaxValue)] long[] tagIds,
            CancellationToken cancellationToken
        )
        {
            var tagsDto = await _querySender.QueryAsync(
                new GetTagsQueryRequest(tagIds),
                cancellationToken
            );
            return Responses.Data(tagsDto);
        }
    }
}
