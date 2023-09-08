using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Response;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]/[action]")]
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

        /// <summary>
        /// Deletes a user account based on their username.
        /// </summary>
        /// <remarks>
        /// Allows administrators to delete a user account identified by their username.
        /// </remarks>
        /// <param name="username">The username of the user to be deleted.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{username:length(2,12)}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The user has been successfully deleted."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified user was not found.",
            typeof(ErrorResponseDto)
        )]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string username, CancellationToken clt)
        {
            // Find the user by their username
            var user = await _userManager.FindByNameAsync(username);

            // Check if the user exists
            if (user is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            }

            // Attempt to delete the user account
            var result = await _userManager.DeleteAsync(user);

            // Check if the deletion was successful
            if (!result.Succeeded)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Delete failed.")
                    .Add(result.Errors)
                    .Build();
            }

            // Return a 204 No Content response indicating successful deletion
            return NoContent();
        }

        /// <summary>
        /// Creates a new role with the specified name.
        /// </summary>
        /// <remarks>
        /// Allows administrators to create a new role with the provided name.
        /// </remarks>
        /// <param name="name">The name of the role to be created.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "OK: The role has been successfully created.",
            typeof(Role)
        )]
        [SwaggerResponse(
            StatusCodes.Status409Conflict,
            "Conflict: A role with the same name already exists.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal Server Error: Role creation failed.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> CreateRole([FromBody] string name, CancellationToken clt)
        {
            // Check if a role with the same name already exists
            var role = await _roleManager.FindByNameAsync(name);
            if (role is not null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status409Conflict, $"There has been a role called '{name}'.")
                    .Build();
            }

            // Create a new role with the provided name
            role = new() { Name = name };

            // Attempt to create the new role
            var result = await _roleManager.CreateAsync(role);

            // Check if role creation was successful
            if (!result.Succeeded)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Create failed.")
                    .Add(result.Errors)
                    .Build();
            }

            // Return the created role
            return ResponseDispatcher.Data(role);
        }

        [HttpPatch]
        public async Task<IActionResult> SetRole(
            [FromBody] RoleSetRequestDto data,
            CancellationToken clt
        )
        {
            var user = await _userManager.FindByNameAsync(data.Username);
            var role = await _roleManager.FindByNameAsync(data.RoleName);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            if (role is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific role.")
                    .Build();
            var result = await _userManager.AddToRoleAsync(user, data.RoleName);
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Add failed.")
                    .Build();
            return NoContent();
        }
    }
}
