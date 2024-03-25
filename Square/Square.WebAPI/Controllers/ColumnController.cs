using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Square.Application.ColumnServices.AddColumn;
using Square.WebAPI.Requests;

namespace Square.WebAPI.Controllers
{
    [Route("api/square")]
    [ApiController]
    public sealed class ColumnController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [Authorize]
        [HttpPost("column/{topicId}")]
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
    }
}
