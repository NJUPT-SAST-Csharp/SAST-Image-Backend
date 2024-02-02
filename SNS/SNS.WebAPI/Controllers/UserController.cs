using Microsoft.AspNetCore.Mvc;
using Primitives.Command;

namespace SNS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ICommandRequestSender commandSender) : ControllerBase { }
}
