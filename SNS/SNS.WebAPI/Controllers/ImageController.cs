using Messenger;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using SNS.Application.ImageServices.AddImage;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ImageController(ICommandRequestSender commandSender) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;

        [NonAction]
        [SubscribeMessage("Image.Add")]
        public async Task AddNewImage(ImageAddedMessage message)
        {
            await _commandSender.CommandAsync(
                new AddImageCommand(message.ImageId, message.AuthorId, message.AlbumId)
            );
        }
    }
}
