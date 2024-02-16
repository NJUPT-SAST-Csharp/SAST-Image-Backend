using Messenger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Shared.Response.Builders;
using SNS.Application.UserServices.AddUser;
using SNS.Application.UserServices.UpdateAvatar;
using SNS.Application.UserServices.UpdateHeader;
using SNS.Application.UserServices.UpdateProfile;
using SNS.WebAPI.Messages;
using SNS.WebAPI.Requests;
using Utilities.Validators;

namespace SNS.WebAPI.Controllers
{
    /// <summary>
    /// The controller to handle user related requests.
    /// </summary>
    [Route("api/sns")]
    [ApiController]
    public class UserController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// The method to handle the user created event.
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

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <remarks>
        /// Update user's nickname and biography.
        /// </remarks>
        /// <param name="request">The new user profile info.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Update profile successfully</response>
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

        /// <summary>
        /// Update User Avatar
        /// </summary>
        /// <remarks>
        /// Update user's avatar image.
        /// </remarks>
        /// <param name="file">The new avatar image file.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Update header successfully</response>
        [Authorize]
        [HttpPut("user/avatar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<NoContent> UpdateAvatar(
            [FromForm][FileValidator(10)] IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            var command = new UpdateAvatarCommand(file, User);

            await _commandSender.CommandAsync(command, cancellationToken);

            return Responses.NoContent;
        }

        /// <summary>
        /// Update User Header
        /// </summary>
        /// <remarks>
        /// Update user's header image, which is displayed on the user's profile page.
        /// <para>Authorization is required</para>
        /// </remarks>
        /// <param name="file">The new header image file.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Update header successfully</response>
        [Authorize]
        [HttpPut("user/header")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<NoContent> UpdateHeader(
            [FromForm][FileValidator(30)] IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            var command = new UpdateHeaderCommand(file, User);

            await _commandSender.CommandAsync(command, cancellationToken);

            return Responses.NoContent;
        }
    }
}
