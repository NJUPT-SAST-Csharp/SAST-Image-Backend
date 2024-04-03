using FoxResult.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using Square.Application.ColumnServices.Queries.GetColumn;
using Square.Application.ColumnServices.Queries.GetColumns;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.ColumnAggregate.Commands.AddColumn;
using Square.Domain.ColumnAggregate.Commands.DeleteColumn;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers
{
    [Route("api/square")]
    [ApiController]
    public sealed class ColumnController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        /// <summary>
        /// Add column.
        /// </summary>
        /// <remarks>
        /// Add a new column to a topic.
        /// </remarks>
        /// <param name="topicId">Id of the target topic</param>
        /// <param name="request">The new column info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">Column added successfully</response>
        /// <response code="404">Topic not found</response>
        [Authorize]
        [HttpPost("topic/{topicId}/column")]
        [DisableRequestSizeLimit]
        [ProducesResponseType<DataResponseType<ColumnId>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IResult> AddColumn(
            [FromRoute] long topicId,
            [FromForm] AddColumnRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _commandSender.CommandAsync(
                new AddColumnCommand(topicId, request.Text, request.Images, User),
                cancellationToken
            );

            var iresult = Results.Extensions.From(result);

            return iresult;
        }

        /// <summary>
        /// Delete column.
        /// </summary>
        /// <remarks>
        /// Delete a column permanently.
        /// </remarks>
        /// <param name="topicId">Id of the topic contains the target column</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Column deleted successfully</response>
        [Authorize]
        [HttpDelete("topic/{topicId}/column")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IResult> DeleteColumn(
            [FromRoute] long topicId,
            CancellationToken cancellationToken = default
        )
        {
            var result = _commandSender.CommandAsync(
                new DeleteColumnCommand(topicId, User),
                cancellationToken
            );

            return Results.Extensions.FromTask(result);
        }

        /// <summary>
        /// Get column.
        /// </summary>
        /// <remarks>
        /// Get a column info with its containing images.
        /// </remarks>
        /// <param name="columnId">Id of the column</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">Column info with its containing images.</response>
        [HttpGet("column/{columnId}")]
        [ProducesResponseType<DataResponseType<ColumnDetailedDto>>(StatusCodes.Status200OK)]
        public Task<IResult> GetColumn(
            [FromRoute] long columnId,
            CancellationToken cancellationToken
        )
        {
            var result = _querySender.QueryAsync(new GetColumnQuery(columnId), cancellationToken);

            return Results.Extensions.FromTask(result);
        }

        /// <summary>
        /// Get columns.
        /// </summary>
        /// <remarks>
        /// Get all columns of a topic.
        /// </remarks>
        /// <param name="topicId">Id of the target topic</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">Columns of the topic.</response>
        [HttpGet("topic/{topicId}/columns")]
        [ProducesResponseType<DataResponseType<IEnumerable<ColumnDto>>>(StatusCodes.Status200OK)]
        public Task<IResult> GetColumns(
            [FromRoute] long topicId,
            CancellationToken cancellationToken
        )
        {
            var result = _querySender.QueryAsync(new GetColumnsQuery(topicId), cancellationToken);

            return Results.Extensions.FromTask(result);
        }
    }
}
