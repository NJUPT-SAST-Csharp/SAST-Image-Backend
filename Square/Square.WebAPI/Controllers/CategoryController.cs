using FoxResult.Extensions;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using Square.Application.CategoryServices.Queries.GetCategories;
using Square.Domain.CategoryAggregate;
using Square.Domain.CategoryAggregate.Commands.CreateCategory;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers
{
    [Route("api/square")]
    [ApiController]
    public sealed class CategoryController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        [HttpPost("category")]
        //[Authorize(nameof(AuthorizationRole.ADMIN))]
        public Task<IResult> CreateCategory(
            [FromBody] CreateCategoryRequest request,
            CancellationToken cancellationToken
        )
        {
            var result = _commandSender.CommandAsync(
                new CreateCategoryCommand(request.Name),
                cancellationToken
            );

            return Results.Extensions.FromTask(result);
        }

        [HttpGet("categories")]
        public Task<IResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = _querySender.QueryAsync(new GetCategoriesQuery(), cancellationToken);

            return Results.Extensions.FromTask(result);
        }

        [HttpPost("test")]
        public Task Test()
        {
            return _commandSender.CommandAsync(new TestCommand());
        }
    }
}
