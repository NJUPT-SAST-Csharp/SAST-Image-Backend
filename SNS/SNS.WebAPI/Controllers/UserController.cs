using Messenger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Shared.Response.Builders;
using SNS.Application.UserServices.AddUser;
using SNS.Application.UserServices.UpdateProfile;
using SNS.WebAPI.Messages;
using SNS.WebAPI.Requests;

namespace SNS.WebAPI.Controllers
{
    [Route("api/sns")]
    [ApiController]
    public class UserController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [NonAction]
        [SubscribeMessage("account.user.created")]
        public async Task UserCreated(
            UserCreatedMessage message,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new AddUserCommand(message.UserId),
                cancellationToken
            );
        }

        [Authorize]
        [HttpPut("user/profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<NoContent> UpdateProfile(
            [FromBody] UpdateProfileRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var command = new UpdateProfileCommand(request.Nickname, request.Biography, User);

            await _commandSender.CommandAsync(command, cancellationToken);

            return Responses.NoContent;
        }

        [Authorize]
        [HttpPut("user/avatar")]
    }
}
