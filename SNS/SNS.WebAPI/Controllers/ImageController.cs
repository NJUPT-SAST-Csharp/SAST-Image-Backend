using Messenger;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using SNS.Application.ImageServices.AddImage;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    [Route("api/sns")]
    [ApiController]
    public sealed class ImageController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [NonAction]
        [SubscribeMessage("sastimg.image.added")]
        public async Task AddNewImage(
            ImageAddedMessage message,
            CancellationToken cancellationToken = default
        )
        {
            await _commandSender.CommandAsync(
                new AddImageCommand(message.ImageId, message.AuthorId, message.AlbumId),
                cancellationToken
            );
        }
    }
}
