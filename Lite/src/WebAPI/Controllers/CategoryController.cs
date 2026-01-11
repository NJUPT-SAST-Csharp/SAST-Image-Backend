using System.ComponentModel.DataAnnotations;
using Application.CategoryServices.Queries;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.CategoryAggregate.Commands;
using Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/categories")]
[ApiController]
[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
public sealed class CategoryController(IMediator mediator) : ControllerBase
{
    #region [Command/Post]

    public readonly record struct CreateCategoryRequest(
        CategoryName Name,
        CategoryDescription Description
    );

    [HttpPost]
    [Authorize(AuthPolicies.Admin)]
    public async Task<IActionResult> Create(
        [FromBody] [Required] CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateCategoryCommand command = new(request.Name, request.Description, User);
        var id = await mediator.Send(command, cancellationToken);
        return Ok(new { id });
    }

    public readonly record struct UpdateCategoryNameRequest(CategoryName Name);

    [HttpPost("{id:long}/name")]
    [Authorize(AuthPolicies.Admin)]
    public async Task<IActionResult> UpdateName(
        [FromRoute] CategoryId id,
        [FromBody] [Required] UpdateCategoryNameRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateCategoryNameCommand command = new(id, request.Name, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateCategoryDescriptionRequest(CategoryDescription Description);

    [HttpPost("{id:long}/description")]
    [Authorize(AuthPolicies.Admin)]
    public async Task<IActionResult> UpdateDescription(
        [FromRoute] CategoryId id,
        [FromBody] [Required] UpdateCategoryDescriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateCategoryDescriptionCommand command = new(id, request.Description, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    //[HttpPost("{id:long}/remove")]
    //public async Task<IActionResult> Remove(
    //    [FromRoute] long id,
    //    CancellationToken cancellationToken
    //)
    //{
    //    var command = new RemoveCategoryCommand(new(id), User);
    //    await mediator.Send(command, cancellationToken);
    //    return NoContent();
    //}

    #endregion

    #region [Query/Get]

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        CategoriesQuery query = new();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    //[HttpGet("{id:long}")]
    //public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
    //{
    //    var query = new GetCategoryQuery(new(id));
    //    var result = await mediator.Send(query, cancellationToken);
    //    return Ok(result);
    //}
    #endregion
}
