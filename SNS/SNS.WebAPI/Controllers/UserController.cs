using Messenger;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using SNS.Application.UserServices.AddUser;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [NonAction]
        [SubscribeMessage("sastimg.user.created")]
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
    }
}
