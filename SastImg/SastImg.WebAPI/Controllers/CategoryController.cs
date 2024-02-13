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
    /// Controller for category related operations.
    /// </summary>
    [ApiController]
    [Route("api/sastimg")]
    [Produces("application/json")]
    public class CategoryController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        /// <summary>
        /// Create Category
        /// </summary>
        /// <remarks>
        /// <para>Create a new category</para>
        /// <para>Admin authorization is required</para>
        /// </remarks>
        /// <param name="request">The new category info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">The category is created</response>
        [Authorize(nameof(AuthorizationRole.Admin))]
        [HttpPost("category")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <remarks>
        /// Get all categories
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The categories</response>
        [HttpGet("categories")]
        [ProducesResponseType<IEnumerable<CategoryDto>>(StatusCodes.Status200OK)]
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
