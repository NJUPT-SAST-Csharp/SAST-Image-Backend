using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Dtos;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<TagDto> _tagValidator;

        public TagController(DatabaseContext dbContext, IValidator<TagDto> tagValidator)
        {
            _dbContext = dbContext;
            _tagValidator = tagValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTag([FromQuery] int id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific tag.")
                    .Build();
            return ResponseDispatcher.Data(new { tag });
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(TagDto newTag)
        {
            var validationResult = await _tagValidator.ValidateAsync(newTag);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            Tag tag = new() { Name = newTag.Name };
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
            return ResponseDispatcher.Data(new { tag });
        }
    }
}
