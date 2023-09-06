using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using static SastImgAPI.Services.ImageAccessor;
using Image = SastImgAPI.Models.DbSet.Image;
using SastImgAPI.Models.ResponseDtos;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<ImageRequestDto> _validator;
        private readonly ImageAccessor _fileAccessor;
        private readonly CacheAuthAccessor _authAccessor;

        public ImageController(
            DatabaseContext dbContext,
            UserManager<User> userManager,
            IValidator<ImageRequestDto> validator,
            ImageAccessor fileAccessor,
            CacheAuthAccessor authAccessor
        )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _validator = validator;
            _fileAccessor = fileAccessor;
            _authAccessor = authAccessor;
        }

        /// <summary>
        /// Retrieve a list of images based on various criteria, including user, tags, and pagination.
        /// </summary>
        /// <remarks>
        /// Allow users to search for images with options to filter by tags, user, and paginate through results.
        /// </remarks>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        /// <param name="username">Optional: The username of the user whose images are being retrieved.</param>
        /// <param name="tags">Optional: An array of tags used to filter images.</param>
        /// <param name="page">Optional: The page number for pagination (default is 0).</param>
        /// <param name="pageSize">Optional: The number of images to retrieve per page (default is 12).</param>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Returns the collection of images based on the specified criteria.",
            typeof(ImageResponseDto[])
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status403Forbidden,
            "Forbidden: Access to the requested images is denied.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The requested user or images are not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> GetImages(
            CancellationToken clt,
            [FromQuery] string? username = default,
            [FromQuery] string[]? tags = default,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 12
        )
        {
            var skipCount = page * pageSize;

            // Create a query for retrieving images with necessary details
            var query = _dbContext.Images
                .Include(image => image.Tags)
                .Include(image => image.Author)
                .OrderBy(image => image.CreatedAt)
                .Select(
                    image =>
                        new ImageResponseDto(
                            image.Id,
                            image.Url,
                            image.Title,
                            image.Description,
                            image.CategoryId,
                            image.CreatedAt,
                            image.Author.UserName!,
                            image.From.Accessibility,
                            image.Tags.Select(tag => tag.Name).ToList()
                        )
                );

            // Filter images based on provided tags
            if (!tags.IsNullOrEmpty())
            {
                query = query.Where(
                    image => image.Tags.Any(imageTag => tags!.Any(tag => tag == imageTag))
                );
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                var isAuthenticated = User.Identity!.IsAuthenticated;

                // Retrieve images based on user authentication and access level
                var images = query
                    .Where(
                        image =>
                            (
                                isAuthenticated
                                && (
                                    image.Accessibility == Accessibility.Everyone
                                    || image.Accessibility == Accessibility.Auth
                                )
                            ) || (!isAuthenticated && image.Accessibility == Accessibility.Everyone)
                    )
                    .Skip(skipCount)
                    .Take(pageSize);

                return ResponseDispatcher.Data(images);
            }

            if (User.FindFirstValue("sub") == username)
            {
                // Retrieve images authored by the authenticated user
                var images = query.Skip(skipCount).Take(pageSize);
                return ResponseDispatcher.Data(images);
            }
            else
            {
                // Retrieve images by username if accessible to others
                var images = query
                    .Where(image => image.Accessibility == Accessibility.Everyone)
                    .Where(image => image.Author == username)
                    .Skip(skipCount)
                    .Take(pageSize);
                return ResponseDispatcher.Data(images);
            }
        }

        /// <summary>
        /// Uploads an image to a user's album.
        /// </summary>
        /// <remarks>
        /// Allow users to upload images to their albums, providing image details such as title, description, and category.
        /// </remarks>
        /// <param name="imageDto">An object containing image information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>

        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the ID of the uploaded image.",
            typeof(ImageCreatedResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status403Forbidden,
            "Forbidden: You don't have permission to upload to the specified album.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified album or category does not exist.",
            typeof(ErrorResponseDto)
        )]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadImage(
            [FromForm] ImageRequestDto imageDto,
            CancellationToken clt
        )
        {
            // Validate the uploaded image details
            var validationResult = await _validator.ValidateAsync(imageDto, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request were invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Retrieve the user's album where the image will be uploaded
            var album = await _dbContext.Albums
                .Where(album => int.Parse(User.FindFirstValue("id")!) == album.AuthorId)
                .Where(album => imageDto.AlbumId == 0 || album.Id == imageDto.AlbumId)
                .Select(
                    album =>
                        new
                        {
                            album.Id,
                            album.Images,
                            album.Name,
                            Author = new
                            {
                                album.Author.Id,
                                Username = album.Author.UserName,
                                album.Author.Nickname
                            }
                        }
                )
                .FirstOrDefaultAsync(clt);

            // Check if the specified album exists
            if (album is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific album.")
                    .Build();

            // Check if the specified category exists
            if (
                await _dbContext.Categories.AllAsync(
                    category => category.Id != imageDto.CategoryId,
                    clt
                )
            )
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status404NotFound,
                        "Couldn't find the specific classification."
                    )
                    .Build();

            // Upload the image and store its URL
            var url = await _fileAccessor.UploadImageAsync(imageDto.ImageFile, clt);

            // Create a new image record
            Image image =
                new()
                {
                    Url = url,
                    AuthorId = album.Author.Id,
                    FromId = album.Id,
                    Title = imageDto.Title,
                    IsExifEnabled = imageDto.IsExifEnabled,
                    Description = imageDto.Description,
                    CategoryId = imageDto.CategoryId,
                };

            // Add the image to the database and save changes
            _dbContext.Images.Add(image);
            await _dbContext.SaveChangesAsync(clt);

            // Return the ID of the uploaded image
            return ResponseDispatcher.Data(new ImageCreatedResponseDto(image.Id));
        }

        /// <summary>
        /// Retrieves detailed information about an image by its unique ID.
        /// </summary>
        /// <remarks>
        /// Allow users to retrieve information about a specific image by providing its unique ID.
        /// </remarks>
        /// <param name="id">The unique identifier of the image to retrieve.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet("{id:int}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns detailed information about the specified image."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image does not exist or is not accessible."
        )]
        public async Task<IActionResult> GetImageById(int id, CancellationToken clt)
        {
            // Determine the user's ID if authenticated, otherwise set to default
            int userId = User.Identity!.IsAuthenticated
                ? int.Parse(User.FindFirstValue("id")!)
                : default;

            // Query the database to retrieve image details by its unique ID
            var image = await _dbContext.Images
                .Select(
                    image =>
                        new ImageDetailedResponseDto(
                            image.Id,
                            image.Title,
                            image.Description,
                            image.Category.Id,
                            image.From.Accessibility,
                            image.IsExifEnabled,
                            image.Tags.Select(tag => tag.Name).ToList(),
                            new(image.Author.Id, image.Author.UserName!, image.Author.Nickname),
                            new(image.From.Id, image.From.Name)
                        )
                )
                .Where(
                    image =>
                        image.Accessibility == Accessibility.Everyone
                        || image.Author.Id == userId
                        || (
                            image.Accessibility == Accessibility.Auth
                            && User.Identity!.IsAuthenticated
                        )
                )
                .FirstOrDefaultAsync(image => image.Id == id, clt);

            // Check if the specified image exists and is accessible
            if (image is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image.")
                    .Build();

            // Return detailed information about the specified image
            return ResponseDispatcher.Data(image);
        }

        /// <summary>
        /// Deletes a specific image by its unique ID.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to delete a specific image by providing its unique ID.
        /// </remarks>
        /// <param name="id">The unique identifier of the image to delete.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The image was successfully deleted."
        )]
        [SwaggerResponse(
            StatusCodes.Status403Forbidden,
            "Forbidden: The user does not have permission to delete this image.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image does not exist.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken clt)
        {
            // Query the database to find the image by its unique ID
            var image = await _dbContext.Images.FirstOrDefaultAsync(image => image.Id == id);

            // Check if the specified image exists
            if (image is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image.")
                    .Build();

            // Check if the user has permission to delete the image (must be the author)
            if (User.FindFirstValue("id") != image.AuthorId.ToString())
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status403Forbidden, "You don't have the access to the image")
                    .Build();
            }

            // Delete the image file from the storage
            _fileAccessor.DeleteImage(image.Url);

            // Remove the image from the database
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync(clt);

            // Return a success status (No Content) indicating the image was deleted
            return NoContent();
        }
    }
}
