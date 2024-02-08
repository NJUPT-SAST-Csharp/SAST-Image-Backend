using Auth.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using SastImg.Application.CategoryServices.CreateCategory;
using SastImg.WebAPI.Requests.CategoryRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api")]
    [ApiController]
    public class CategoryController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// TODO: complete
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Authorize(nameof(AuthorizationRole.Admin))]
        [HttpPost("category")]
        public async Task<NoContent> CreateCategory(
            [FromBody] CreateCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            await _commandSender.CommandAsync(
                new CreateCategoryCommand(request.Name, request.Description),
                cancellationToken
            );

            return Responses.NoContent;
        }
    }
}
