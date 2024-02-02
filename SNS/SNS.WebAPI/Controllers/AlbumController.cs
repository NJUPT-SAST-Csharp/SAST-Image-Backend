using Messenger;
using Microsoft.AspNetCore.Mvc;
using SNS.WebAPI.Messages;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        [NonAction]
        [SubscribeMessage("sastimg.album.created")]
        public async Task AlbumCreated(AlbumCreatedMessage message) { }
    }
}
