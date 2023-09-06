using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.ResponseDtos;
using Swashbuckle.AspNetCore.Annotations;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<TagRequestDto> _tagValidator;

        public TagController(
            DatabaseContext dbContext,
            IValidator<TagRequestDto> tagValidator,
            CancellationToken clt
        )
        {
            _dbContext = dbContext;
            _tagValidator = tagValidator;
        }

        /// <summary>
        /// Retrieves tag information by its name.
        /// </summary>
        /// <remarks>
        /// Allow users to retrieve information about a tag based on its name. The name of the tag
        /// is provided as a route parameter. The endpoint queries the database for the tag with the specified name.
        /// If the tag is found, its details are returned in the response. If not found, a  not found response is returned.
        /// </remarks>
        /// <param name="name">The name of the tag to retrieve. Must be between 2 and 6 characters in length.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>

        [HttpGet("{name:length(2,6)}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns the specified tag.",
            typeof(TagResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified tag could not be found in the database.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> GetTag(string name, CancellationToken clt)
        {
            // Query the database to find a tag with the specified name
            var tag = await _dbContext.Tags
                .Select(tag => new TagResponseDto(tag.Id, tag.Name))
                .FirstOrDefaultAsync(x => x.Name == name, clt);

            // Check if the tag was found
            if (tag is null)
            {
                // Return a "404 Not Found" response if the tag is not found
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific tag.")
                    .Build();
            }

            // Return a JSON response with information about the retrieved tag
            return ResponseDispatcher.Data(tag);
        }

        /// <summary>
        /// Retrieves a list of all tags in the database.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to retrieve a list of all tags present in the database.
        /// It queries the database and retrieves all available tags, which are returned in the response.
        /// </remarks>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns all available tags.",
            typeof(TagResponseDto[])
        )]
        public async Task<IActionResult> GetAllTags(CancellationToken clt)
        {
            // Query the database to retrieve all available tags
            var tags = await _dbContext.Tags
                .Select(tag => new TagResponseDto(tag.Id, tag.Name))
                .ToListAsync(clt);

            // Return a JSON response containing the list of tags
            return ResponseDispatcher.Data(tags);
        }

        /// <summary>
        /// Adds a new tag to the database.
        /// </summary>
        /// <remarks>
        /// Allow authenticated users to add a new tag to the database.
        /// It first validates the provided tag data to ensure it is valid.
        /// If the tag is valid and doesn't already exist in the database, it is added, and a success response is returned.
        /// If the tag name already exists, a conflict response is returned.
        /// </remarks>
        /// <param name="newTag">A <see cref="TagRequestDto"/> object containing the new tag's information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [Authorize(Roles = "User")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Return the tag successfully added to the database.",
            typeof(TagResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status409Conflict,
            "Conflict: A tag with the same name already exists in the database.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> AddTag(TagRequestDto newTag, CancellationToken clt)
        {
            // Validate the provided tag data
            var validationResult = await _tagValidator.ValidateAsync(newTag, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Check if a tag with the same name already exists in the database
            if (await _dbContext.Tags.AnyAsync(tag => tag.Name == newTag.Name, clt))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status409Conflict,
                        "A tag with the name '" + newTag.Name + "' already exists."
                    )
                    .Build();

            // Create a new Tag instance and add it to the database
            Tag tag = new() { Name = newTag.Name };
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync(clt);

            // Return a JSON response containing the newly added tag's information
            return ResponseDispatcher.Data(new TagResponseDto(tag.Id, tag.Name));
        }

        /// <summary>
        /// Deletes a tag from the database.
        /// </summary>
        /// <remarks>
        /// Allow authenticated administrators to delete a tag from the database.
        /// It first looks for the tag with the provided name in the database.
        /// If the tag is found, it is removed from the database, and a success response is returned.
        /// If the tag does not exist, a not found response is returned.
        /// </remarks>
        /// <param name="name">The name of the tag to be deleted (2 to 6 characters in length).</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{name:length(2,6)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The tag was successfully deleted from the database."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified tag does not exist in the database.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> DeleteTag(string name, CancellationToken clt)
        {
            // Look for the tag with the provided name in the database
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Name == name);

            // If the tag is not found, return a '404 Not Found' response
            if (tag is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific tag.")
                    .Build();

            // Remove the tag from the database
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync(clt);

            // Return a '204 No Content' response indicating successful deletion
            return NoContent();
        }
    }
}
