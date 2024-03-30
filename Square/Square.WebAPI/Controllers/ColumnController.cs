using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Square.Domain.ColumnAggregate.Commands.AddColumn;
using Square.Domain.ColumnAggregate.Commands.DeleteColumn;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers
{
    [Route("api/square")]
    [ApiController]
    public sealed class ColumnController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [Authorize]
        [HttpPost("topic/{topicId}/column")]
        [DisableRequestSizeLimit]
        public Task AddColumn(
            [FromRoute] long topicId,
            [FromForm] AddColumnRequest request,
            CancellationToken cancellationToken = default
        )
        {
            return _commandSender.CommandAsync(
                new AddColumnCommand(topicId, request.Text, request.Images, User),
                cancellationToken
            );
        }

        [Authorize]
        [HttpDelete("topic/{topicId}/column")]
        public Task DeleteColumn(
            [FromRoute] long topicId,
            CancellationToken cancellationToken = default
        )
        {
            return _commandSender.CommandAsync(
                new DeleteColumnCommand(topicId, User),
                cancellationToken
            );
        }
    }
}
