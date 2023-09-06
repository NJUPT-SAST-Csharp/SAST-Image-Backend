using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using SastImgAPI.Models.ResponseDtos;

namespace SastImgAPI.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<ProfileRequestDto> _validator;
        private readonly ImageAccessor _imageAccessor;

        public ProfileController(
            UserManager<User> userManager,
            DatabaseContext dbContext,
            IValidator<ProfileRequestDto> validator,
            ImageAccessor imageAccessor
        )
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _validator = validator;
            _imageAccessor = imageAccessor;
        }

        /// <summary>
        /// Retrieves the profile information of a specific user by their username.
        /// </summary>
        /// <remarks>
        /// Allow users to retrieve the profile information of a specific user by providing their username.
        /// </remarks>
        /// <param name="username">The username of the user whose profile information is being retrieved.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet("{username:length(2,12)}")]
        [AllowAnonymous]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the user's profile information.",
            typeof(ProfileResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified user does not exist.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> GetProfile(string username, CancellationToken clt)
        {
            // Query the user manager to find the user by their username
            var user = await _userManager.FindByNameAsync(username);

            // Check if the specified user exists
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();

            // Include profile information
            return ResponseDispatcher.Data(
                new ProfileResponseDto(
                    user.UserName!,
                    user.Nickname,
                    user.Email,
                    user.Biography,
                    user.Website,
                    user.Avatar,
                    user.Header,
                    user.RegisteredAt
                )
            );
        }

        /// <summary>
        /// Updates the profile information of the authenticated user.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to update their profile information, including their nickname,
        /// biography, and website. The new profile information is provided in the request body as a JSON object.
        /// </remarks>
        /// <param name="newProfile">An object containing the updated profile information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPut]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The user's profile information was successfully updated."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The authenticated user could not be found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] ProfileRequestDto newProfile,
            CancellationToken clt
        )
        {
            // Validate the updated profile information
            var validationResult = await _validator.ValidateAsync(newProfile, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Retrieve the authenticated user by their user ID
            var user = await _dbContext.Users.FirstOrDefaultAsync(
                user => user.Id == int.Parse(User.FindFirstValue("id")!),
                clt
            );

            // Check if the authenticated user exists
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();

            // Update the user's profile information with the new data
            user.Nickname = newProfile.Nickname;
            user.Biography = newProfile.Biography;
            user.Website = newProfile.Website;

            // Save the changes to the database
            await _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response to indicate a successful update
            return NoContent();
        }

        /// <summary>
        /// Uploads a new avatar image for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to upload a new avatar image. The uploaded image should be in
        /// a supported format and sent as a multi-part form data request. Upon successful upload, the user's
        /// profile avatar URL is updated, and the new avatar URL is returned in the response.
        /// </remarks>
        /// <param name="avatar">The avatar image to be uploaded as a multi-part form data.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="404"></response>
        [HttpPost("Avatar")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the uploaded avatar image's URL.",
            typeof(UrlResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The authenticated user could not be found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> UploadAvatar(
            [FromForm] IFormFile avatar,
            CancellationToken clt
        )
        {
            // Retrieve the authenticated user by their user ID
            var user = await _userManager.FindByIdAsync(User.FindFirstValue("id")!);

            // Check if the authenticated user exists
            if (user == null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            }

            // Upload the new avatar image and get its URL
            var url = await _imageAccessor.UploadAvatarAsync(
                avatar,
                int.Parse(User.FindFirstValue("id")!),
                clt
            );

            // Update the user's avatar URL with the new URL
            user!.Avatar = url;

            // Save the changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a JSON response with the URL of the newly uploaded avatar
            return ResponseDispatcher.Data(new UrlResponseDto(url));
        }

        /// <summary>
        /// Uploads a new header image for the authenticated user's profile.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to upload a new header image for their profile. The uploaded
        /// image should be in a supported format and sent as a multi-part form data request. Upon successful upload,
        /// the user's profile header URL is updated, and the new header URL is returned in the response.
        /// </remarks>
        /// <param name="header">The header image to be uploaded as multi-part form data.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="404"></response>

        [HttpPost("Header")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the uploaded header image's URL.",
            typeof(UrlResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The authenticated user could not be found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> UploadHeader(
            [FromForm] IFormFile header,
            CancellationToken clt
        )
        {
            // Retrieve the authenticated user by their user ID
            var user = await _userManager.FindByIdAsync(User.FindFirstValue("id")!);

            // Check if the authenticated user exists
            if (user == null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            }

            // Upload the new header image and get its URL
            var url = await _imageAccessor.UploadHeaderAsync(
                header,
                int.Parse(User.FindFirstValue("id")!),
                clt
            );

            // Update the user's header URL with the new URL
            user!.Header = url;

            // Save the changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a JSON response with the URL of the newly uploaded header
            return ResponseDispatcher.Data(new UrlResponseDto(url));
        }
    }
}
