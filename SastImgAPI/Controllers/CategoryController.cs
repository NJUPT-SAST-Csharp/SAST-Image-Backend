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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<CategoryRequestDto> _validator;

        public CategoryController(
            DatabaseContext dbContext,
            IValidator<CategoryRequestDto> validator
        )
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves a list of all available categories.
        /// </summary>
        /// <remarks>
        /// Allows users to fetch a list of all available categories.
        /// </remarks>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "OK: List of categories successfully retrieved.",
            typeof(CategoryResponseDto[])
        )]
        public async Task<IActionResult> GetCategories(CancellationToken clt)
        {
            // Retrieve a list of all available categories
            var categories = await _dbContext.Categories
                .Select(category => new CategoryResponseDto(category))
                .ToListAsync(clt);

            // Return the list of categories
            return ResponseDispatcher.Data(categories);
        }

        /// <summary>
        /// Retrieves category information by its unique name.
        /// </summary>
        /// <remarks>
        /// Allows users to fetch information about a specific category based on its unique name.
        /// </remarks>
        /// <param name="name">The unique name of the category.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet("{name:length(2,10)}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "OK: Category information successfully retrieved.",
            typeof(CategoryResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified category was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> GetCategory(string name, CancellationToken clt)
        {
            // Find the category by its unique name
            var category = await _dbContext.Categories.FirstOrDefaultAsync(
                category => category.Name == name,
                clt
            );

            // Check if the category exists
            if (category is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "The specified category was not found.")
                    .Build();
            }

            // Return the category information
            return ResponseDispatcher.Data(new CategoryResponseDto(category));
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <remarks>
        /// Allows administrators to create a new category using the provided data.
        /// </remarks>
        /// <param name="category">The CategoryRequestDto containing the category data.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Created: The category has been successfully created.",
            typeof(CategoryResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status409Conflict,
            "Conflict: A category with the same name already exists.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Post(
            [FromBody] CategoryRequestDto category,
            CancellationToken clt
        )
        {
            // Validate the incoming category data
            var validationResult = await _validator.ValidateAsync(category, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request are invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Check for conflicts with existing category names
            var isConflict = await _dbContext.Categories
                .Select(existingCategory => existingCategory.Name)
                .AnyAsync(name => name == category.Name);

            if (isConflict)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status409Conflict,
                        "A category with the same name already exists."
                    )
                    .Build();

            // Create a new Category instance with the provided data
            Category newCategory =
                new() { Name = category.Name, Description = category.Description };

            // Add the new category to the database and save changes
            _dbContext.Categories.Add(newCategory);
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 200 Ok response with the newly created category's information
            return ResponseDispatcher.Data(new CategoryResponseDto(newCategory));
        }

        /// <summary>
        /// Modifies an existing category's information based on its name, only accessible to users with 'Admin' role.
        /// </summary>
        /// <remarks>
        /// Allows authorized users with the 'Admin' role to modify the details of a category identified by its name.
        /// </remarks>
        /// <param name="name">The name of the category to be modified.</param>
        /// <param name="categoryDto">The new category information to replace the existing one.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPut("{name:length(2,10)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The category has been successfully modified."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified category was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> ModifyCategory(
            string name,
            [FromBody] CategoryRequestDto categoryDto,
            CancellationToken clt
        )
        {
            // Validate the incoming data
            var validationResult = await _validator.ValidateAsync(categoryDto, clt);
            if (!validationResult.IsValid)
            {
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request are invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();
            }

            // Find the category by its name
            var category = await _dbContext.Categories.FirstOrDefaultAsync(
                c => c.Name == name,
                clt
            );

            // Check if the category exists
            if (category is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific category.")
                    .Build();
            }

            // Update the category's information
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            // Save changes to the database
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating successful modification
            return NoContent();
        }

        /// <summary>
        /// Deletes a category based on its unique name.
        /// </summary>
        /// <remarks>
        /// Allows administrators to delete a category identified by its unique name.
        /// </remarks>
        /// <param name="name">The unique name of the category to be deleted.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpDelete("{name:length(2,10)}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The category has been successfully deleted."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Not Found: The specified category was not found.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Delete(string name, CancellationToken clt)
        {
            // Find the category by its unique name
            var category = await _dbContext.Categories.FirstOrDefaultAsync(
                category => category.Name == name,
                clt
            );

            // Check if the category exists
            if (category is null)
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "The specified category was not found.")
                    .Build();
            }

            // Remove the category from the database and save changes
            _dbContext.Remove(category);
            _ = _dbContext.SaveChangesAsync(clt);

            // Return a 204 No Content response indicating successful deletion
            return NoContent();
        }
    }
}
