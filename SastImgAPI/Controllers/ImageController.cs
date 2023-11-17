using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Response;
using Response.Builders;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.ResponseDtos;
using SastImgAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using Image = SastImgAPI.Models.DbSet.Image;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<ImageRequestDto> _validator;
        private readonly ImageAccessor _fileAccessor;

        public ImageController(
            DatabaseContext dbContext,
            IValidator<ImageRequestDto> validator,
            ImageAccessor fileAccessor
        )
        {
            _dbContext = dbContext;
            _validator = validator;
            _fileAccessor = fileAccessor;
        }

        /// <summary>
        /// Retrieve a list of images based on various criteria, including user, tags, and pagination.
        /// </summary>
        /// <remarks>
        /// Allow users to search for images with options to filter by tags, user, and paginate through results.
        /// </remarks>
        /// <param name="cancellationToken">A CancellationToken used for canceling the operation.</param>
        /// <param name="username">Optional: The username of the user whose images are being retrieved.</param>
        /// <param name="category">Optional: The name of category used to filter images.</param>
        /// <param name="tags">Optional: An array of tags used to filter images.</param>
        /// <param name="page">Optional: The page number for pagination (default is 0).</param>
        /// <param name="pageSize">Optional: The number of images to retrieve per page (default is 12).</param>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the collection of images based on the specified criteria.",
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
            CancellationToken cancellationToken,
            [FromQuery] string? category = null,
            [FromQuery] string? username = null,
            [FromQuery(Name = "tag")] string[]? tags = default,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 12
        )
        {
            var skipCount = page * pageSize;

            // Create a query for retrieving images with necessary details
            IQueryable<Image> query = _dbContext
                .Images
                .Include(image => image.Author)
                .Include(image => image.Album);

            // Filter images based on provided category.
            if (category is not null)
            {
                query = query.Where(image => image.Category.Id == CodeAccessor.ToLongId(category));
            }
            if (User.Identity!.IsAuthenticated)
            {
                // Retrieve images based on user authentication and access level
                query = query.Where(
                    image =>
                        (
                            // Retrieve images authored by the authenticated user
                            User.FindFirstValue("username") == username
                            && image.Author.UserName == username
                        )
                        || (
                            // Retrieve images by other authenticated users if accessible to them
                            (username.IsNullOrEmpty() || image.Author.UserName == username)
                            && (
                                image.Album.Accessibility == Accessibility.Everyone
                                || image.Album.Accessibility == Accessibility.Auth
                            )
                        )
                );
            }
            else
            {
                // Retrieve images by unauthenticated users
                query = query.Where(
                    image =>
                        image.Album.Accessibility == Accessibility.Everyone
                        && (username.IsNullOrEmpty() || image.Author.UserName == username)
                );
            }

            // Filter images based on provided tags
            if (!tags.IsNullOrEmpty())
            {
                foreach (long tagId in tags!.Select(CodeAccessor.ToLongId))
                {
                    query = query.Where(image => image.Tags.Any(imageTag => imageTag.Id == tagId));
                }
            }

            var images = await query
                .OrderBy(image => image.CreatedAt)
                .Skip(skipCount)
                .Take(pageSize)
                .Select(image => new ImageResponseDto(image))
                .ToListAsync();

            return ReponseBuilder.Data(images);
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
                return ReponseBuilder
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request were invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            await Console.Out.WriteLineAsync(User.FindFirstValue("id"));
            // Retrieve the user's album where the image will be uploaded
            var album = await _dbContext
                .Albums
                .Where(album => CodeAccessor.ToLongId(User.FindFirstValue("id")!) == album.AuthorId)
                .Where(
                    album =>
                        !imageDto.AlbumId.IsNullOrEmpty()
                        && album.Id == CodeAccessor.ToLongId(imageDto.AlbumId)
                )
                .Select(
                    album =>
                        new
                        {
                            album.Id,
                            album.Name,
                            album.AuthorId
                        }
                )
                .FirstOrDefaultAsync(clt);

            // Check if the specified album exists
            if (album is null)
                return ReponseBuilder
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific album.")
                    .Build();

            // Check if the specified category exists
            var category = await _dbContext
                .Categories
                .FirstOrDefaultAsync(
                    category => category.Id == CodeAccessor.ToLongId(imageDto.Category),
                    clt
                );
            if (category is null)
                return ReponseBuilder
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific category.")
                    .Build();

            // Upload the image and store its URL
            string url = await _fileAccessor.UploadImageAsync(imageDto.ImageFile, clt);

            // Create a new image record
            Image image =
                new()
                {
                    Url = new Uri(url),
                    AuthorId = album.AuthorId,
                    AlbumId = album.Id,
                    Title = imageDto.Title,
                    IsExifEnabled = imageDto.IsExifEnabled,
                    Description = imageDto.Description,
                    Category = category
                };

            if (!imageDto.Tags.IsNullOrEmpty())
            {
                // Retrieve the necessary tags
                var tags = await _dbContext
                    .Tags
                    .Where(tag => imageDto.Tags!.Contains(tag.Name))
                    .ToListAsync(clt);
                // Add tags to the uploaded image
                foreach (Tag tag in tags)
                    image.Tags.Add(tag);
            }

            // Add the image to the database and save changes
            _dbContext.Images.Add(image);
            await _dbContext.SaveChangesAsync(clt);

            // Return the ID of the uploaded image
            return ReponseBuilder.Data(new ImageCreatedResponseDto(image));
        }

        /// <summary>
        /// Retrieves detailed information about an image by its unique ID.
        /// </summary>
        /// <remarks>
        /// Allow users to retrieve information about a specific image by providing its unique ID.
        /// </remarks>
        /// <param name="id">The unique identifier of the image to retrieve.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet("{id:length(11)}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns detailed information about the specified image."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image does not exist or is not accessible."
        )]
        public async Task<IActionResult> GetImageById(string id, CancellationToken clt)
        {
            long userId = User.Identity!.IsAuthenticated
                ? CodeAccessor.ToLongId(User.FindFirstValue("id")!)
                : 0;

            // Query the database to retrieve image details by its unique ID
            var image = await _dbContext
                .Images
                .Where(
                    image =>
                        image.Album.Accessibility == Accessibility.Everyone
                        || image.Author.Id == userId
                        || (
                            image.Album.Accessibility == Accessibility.Auth
                            && User.Identity!.IsAuthenticated
                        )
                )
                .Select(image => new ImageDetailedResponseDto(image))
                .FirstOrDefaultAsync(image => image.Id == id, clt);

            // Check if the specified image exists and is accessible
            if (image is null)
                return ReponseBuilder
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image.")
                    .Build();

            // Return detailed information about the specified image
            return ReponseBuilder.Data(image);
        }

        /// <summary>
        /// Deletes an image based on its unique ID, with specific authorization requirements.
        /// </summary>
        /// <remarks>
        /// Allows authorized users with the 'User' role to delete their own images. Users with the 'Admin' role
        /// have the privilege to delete any image.
        /// </remarks>
        /// <param name="id">The unique ID of the image to be deleted.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{id:length(11)}")]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The image has been successfully deleted."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified image was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> DeleteImage(string id, CancellationToken clt)
        {
            // Find the image based on the user's authorization level
            var image = await _dbContext
                .Images
                .Where(
                    image =>
                        image.AuthorId == CodeAccessor.ToLongId(User.FindFirstValue("id")!) // User is the owner
                        || User.Claims
                            .Where(claim => claim.Type == "role")
                            .Select(claim => claim.Value)
                            .Contains("Admin") // User has Admin role
                )
                .FirstOrDefaultAsync(image => image.Id == CodeAccessor.ToLongId(id), clt);

            // Check if the image exists
            if (image is null)
            {
                return ReponseBuilder
                    .Error(StatusCodes.Status404NotFound, "The specified image was not found.")
                    .Build();
            }

            // Delete the image file from storage
            _fileAccessor.DeleteImage(image.Url.ToString());

            // Remove the image from the database and save changes
            _dbContext.Images.Remove(image);
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating successful deletion
            return NoContent();
        }
    }
}
