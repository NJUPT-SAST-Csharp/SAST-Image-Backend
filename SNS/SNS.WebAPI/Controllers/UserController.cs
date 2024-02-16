using Messenger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using Shared.Response.Builders;
using SNS.Application.UserServices.AddUser;
using SNS.Application.UserServices.GetUser;
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

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <remarks>
        /// Update user's nickname and biography.
        /// <para>Authorization is required</para>
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
        /// <para>Authorization is required</para>
        /// </remarks>
        /// <param name="file">The new avatar image file.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="204">Update header successfully</response>
        [Authorize]
        [HttpPut("user/avatar")]
        [Produces("application/json", "multipart/form-data")]
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
        [Produces("application/json", "multipart/form-data")]
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

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <remarks>
        /// Get user's profile info,
        /// including nickname, biography, avatar and header image.
        /// </remarks>
        /// <param name="userId">The user's id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">The user's profile info</response>
        /// <response code="404">The user is not found</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Results<Ok<UserDto>, NotFound>> GetUser(
            [FromRoute] long userId,
            CancellationToken cancellationToken = default
        )
        {
            var user = await _querySender.QueryAsync(new GetUserQuery(userId), cancellationToken);

            return Responses.DataOrNotFound(user);
        }
    }
}
