using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Response;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpDelete("User")]
        public async Task<IActionResult> DeleteUser([FromQuery] string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return ResponseDispatcher.Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.").Build();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return ResponseDispatcher.Error(StatusCodes.Status400BadRequest, "Delete failed.").Build();

            return NoContent();
        }

        [HttpPost("Role")]
        public async Task<IActionResult> CreateRole([FromBody] string name, CancellationToken clt)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role is not null)
                return ResponseDispatcher.Error(StatusCodes.Status409Conflict, $"There has been a role called '{name}'.").Build();
            role = new() { Name = name };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return ResponseDispatcher.Error(StatusCodes.Status400BadRequest, "Create failed.").Build();
            return ResponseDispatcher.Data(role);
        }

        [AllowAnonymous]
        [HttpPut("User")]
        public async Task<IActionResult> SetRoleForUser([FromBody] RoleSetRequestDto data, CancellationToken clt)
        {
            var user = await _userManager.FindByNameAsync(data.Username);
            var role = await _roleManager.FindByNameAsync(data.RoleName);
            if (user is null)
                return ResponseDispatcher.Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.").Build();
            if (role is null)
                return ResponseDispatcher.Error(StatusCodes.Status404NotFound, "Couldn't find the specific role.").Build();
            var result = await _userManager.AddToRoleAsync(user, data.RoleName);
            if (!result.Succeeded)
                return ResponseDispatcher.Error(StatusCodes.Status400BadRequest, "Add failed.").Build();
            return NoContent();
        }
    }
}
