using Identity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SastImg.Application.CategoryServices.CreateCategory;
using SastImg.Application.CategoryServices.GetAllCategory;
using SastImg.Domain.CategoryEntity;
using Shared.Response.Builders;

namespace SastImg.WebAPI.Controllers;

/// <summary>
/// Controller for category related operations.
/// </summary>
[ApiController]
[Route("api/sastimg")]
[Produces("application/json")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    public readonly record struct CreateCategoryRequest(
        CategoryName Name,
        CategoryDescription Description
    );

    /// <summary>
    /// Create Category
    /// </summary>
    /// <remarks>
    /// Create a new category
    /// <para>ADMIN authorization is required</para>
    /// </remarks>
    /// <param name="request">The new category info</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="204">The category is created</response>
    [Authorize(nameof(Role.ADMIN))]
    [HttpPost("category")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContent> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(
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
        var categories = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Responses.Data(categories);
    }
}
