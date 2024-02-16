using Messenger;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SNS.Application.UserServices.AddUser;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    /// <summary>
    /// The controller to handle user related requests.
    /// </summary>
    [Route("api/sns")]
    [ApiController]
    [Produces("application/json")]
    public class UserController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        /// <summary>
        /// The method handles the user created event.
        /// </summary>
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
    }
}
