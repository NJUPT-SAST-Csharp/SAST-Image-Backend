using Messenger;
using Microsoft.AspNetCore.Mvc;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        [NonAction]
        [SubscribeMessage("")]
        public Task AlbumCreated() { }
    }
}
