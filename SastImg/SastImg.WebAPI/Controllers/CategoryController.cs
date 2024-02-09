using Auth.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SastImg.Application.CategoryServices.CreateCategory;
using SastImg.Application.CategoryServices.GetAllCategory;
using SastImg.WebAPI.Requests.CategoryRequest;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    [Route("api/sastimg")]
    [ApiController]
    public class CategoryController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

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
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new CreateCategoryCommand(request.Name, request.Description),
                cancellationToken
            );

            return Responses.NoContent;
        }

        [HttpGet("categories")]
        public async Task<Ok<IEnumerable<CategoryDto>>> GetAllCategories(
            CancellationToken cancellationToken = default
        )
        {
            var categories = await _querySender.QueryAsync(
                new GetAllCategoriesQuery(),
                cancellationToken
            );
            return Responses.Data(categories);
        }
    }
}
