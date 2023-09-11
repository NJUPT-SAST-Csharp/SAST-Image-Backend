using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ContactController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Likes an image by its unique ID if the user is authenticated.
        /// </summary>
        /// <remarks>
        /// Allows an authenticated user to like an image by providing the image's unique ID.
        /// If the user is not authenticated or if the image does not exist, an error response will be returned.
        /// If the user has already liked the image, the request is considered successful, but no changes are made.
        /// </remarks>
        /// <param name="id">The unique ID of the image to be liked.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost("{id:length(11)}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The image has been successfully liked."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Like(string id, CancellationToken clt)
        {
            // Find the likes for the specified image by its unique ID
            var likes = await _dbContext.Images
                .Where(image => image.Id == CodeAccessor.ToLongId(id))
                .Select(image => image.LikedBy)
                .FirstOrDefaultAsync(clt);

            // Check if the image exists
            if (likes is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image")
                    .Build();
            }

            // Get the authenticated user's ID from the claims
            var userId = CodeAccessor.ToLongId(User.FindFirstValue("id")!);

            // Check if the user has already liked the image
            if (!likes.Contains(userId))
            {
                // If not, add the user's ID to the list of likes
                likes.Add(userId);
            }

            // Save the changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating success
            return NoContent();
        }

        /// <summary>
        /// Unlikes an image by its unique ID if the user is authenticated and has previously liked it.
        /// </summary>
        /// <remarks>
        /// Allows an authenticated user to unlike an image by providing the image's unique ID.
        /// If the user is not authenticated, if the image does not exist, or if the user has not previously liked the image,
        /// the request is considered successful, but no changes are made.
        /// </remarks>
        /// <param name="id">The unique ID of the image to be unliked.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{id:length(11)}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No Content: The image has been unliked.")]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Unlike(string id, CancellationToken clt)
        {
            // Find the likes for the specified image by its unique ID
            var likes = await _dbContext.Images
                .Where(image => image.Id == CodeAccessor.ToLongId(id))
                .Select(image => image.LikedBy)
                .FirstOrDefaultAsync(clt);

            // Check if the image exists
            if (likes is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image")
                    .Build();
            }

            // Get the authenticated user's ID from the claims
            var userId = CodeAccessor.ToLongId(User.FindFirstValue("id")!);

            // Check if the user has previously liked the image
            if (likes.Contains(userId))
            {
                // If yes, remove the user's ID from the list of likes to unlike the image
                likes.Remove(userId);
            }

            // Save the changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating success
            return NoContent();
        }

        /// <summary>
        /// Increments the view count of an image based on its unique ID.
        /// </summary>
        /// <remarks>
        /// Allows users to add a view to an image by providing its unique ID.
        /// The view count of the image is incremented by one.
        /// </remarks>
        /// <param name="id">The unique ID of the image to add a view to.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPatch("{id:length(11)}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The view count has been successfully updated."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> AddViewCount(string id, CancellationToken clt)
        {
            int views;
            try
            {
                views = await _dbContext.Images
                    .Where(image => image.Id == CodeAccessor.ToLongId(id))
                    .Select(image => image.Views)
                    .FirstAsync(clt);
            }
            catch
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "The specified image was not found.")
                    .Build();
            }

            views++;

            // Save the changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            return NoContent();
        }
    }
}
