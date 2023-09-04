using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Dtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using System.Security.Claims;
using static SastImgAPI.Services.ImageAccessor;
using Image = SastImgAPI.Models.DbSet.Image;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<ImageDto> _validator;
        private readonly ImageAccessor _fileAccessor;
        private readonly CacheAuthAccessor _authAccessor;

        public ImageController(
            DatabaseContext dbContext,
            UserManager<User> userManager,
            IValidator<ImageDto> validator,
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

        [HttpGet]
        public async Task<IActionResult> GetImages(
            CancellationToken clt,
            [FromQuery] string? username = default,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 12
        )
        {
            var skipCount = page * pageSize;
            var temp = _dbContext.Images
                .Include(image => image.Tags)
                .OrderBy(image => image.CreatedAt)
                .Select(
                    image =>
                        new
                        {
                            image.Id,
                            image.Url,
                            image.Title,
                            image.Description,
                            image.CategoryId,
                            image.CreatedAt,
                            image.AuthorId,
                            image.From.Accessibility,
                            Tags = image.Tags.Select(tag => tag.Name)
                        }
                );
            if (string.IsNullOrWhiteSpace(username))
            {
                var images = temp.Where(image => image.Accessibility == Accessibility.Everyone)
                    .Skip(skipCount)
                    .Take(pageSize);
                return ResponseDispatcher.Data(images);
            }

            var user = await _userManager.FindByNameAsync(username!);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            if (User.FindFirstValue("sub") == username)
            {
                var images = temp.Skip(skipCount).Take(pageSize);
                return ResponseDispatcher.Data(images);
            }
            else
            {
                var images = temp.Where(image => image.Accessibility == Accessibility.Everyone)
                    .Where(image => image.AuthorId == user.Id)
                    .Skip(skipCount)
                    .Take(pageSize);
                return ResponseDispatcher.Data(new { images });
            }
        }

        //[Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(
            [FromForm] ImageDto imageDto,
            CancellationToken clt
        )
        {
            var validationResult = await _validator.ValidateAsync(imageDto, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

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
                                album.Author.UserName,
                                album.Author.Nickname
                            }
                        }
                )
                .FirstOrDefaultAsync(clt);

            if (album is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific album.")
                    .Build();
            if (
                await _dbContext.Categories.AllAsync(category => category.Id != imageDto.CategoryId)
            )
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status404NotFound,
                        "Couldn't find the specific classification."
                    )
                    .Build();

            var url = await _fileAccessor.UploadImageAsync(imageDto.ImageFile, clt);
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

            _dbContext.Images.Add(image);
            await _dbContext.SaveChangesAsync(clt);
            return ResponseDispatcher.Data(new { image.Id, });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetImage(int id, CancellationToken clt)
        {
            int userId = User.Identity!.IsAuthenticated
                ? int.Parse(User.FindFirstValue("id")!)
                : default;

            var image = await _dbContext.Images
                .Select(
                    image =>
                        new
                        {
                            image.Id,
                            image.Title,
                            image.Description,
                            Classification = image.Category.Id,
                            image.From.Accessibility,
                            image.IsExifEnabled,
                            Author = new
                            {
                                image.Author.Id,
                                image.Author.UserName,
                                image.Author.Nickname
                            },
                            From = new { image.From.Id, image.From.Name, }
                        }
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

            if (image is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image.")
                    .Build();

            return ResponseDispatcher.Data(image);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken clt)
        {
            var image = await _dbContext.Images.FirstOrDefaultAsync(image => image.Id == id);

            if (image is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific image.")
                    .Build();
            if (User.FindFirstValue("id") != image.AuthorId.ToString())
            {
                return ResponseDispatcher
                    .Error(StatusCodes.Status403Forbidden, "You don't have the access to the image")
                    .Build();
            }
            _fileAccessor.DeleteImage(image.Url);
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync(clt);

            return NoContent();
        }
    }
}
