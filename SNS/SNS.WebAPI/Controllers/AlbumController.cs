﻿using Messenger;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using SNS.Application.AlbumServices.AddAlbum;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    /// <summary>
    /// Controller for album-related operations.
    /// </summary>
    [Route("api/sns")]
    [ApiController]
    public class AlbumController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        /// <summary>
        /// The method handles the user created event.
        /// </summary>
        [NonAction]
        [SubscribeMessage("sastimg.album.created")]
        public async Task AlbumCreated(
            AlbumCreatedMessage message,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new AddAlbumCommand(message.AlbumId, message.AuthorId),
                cancellationToken
            );
        }
    }
}
