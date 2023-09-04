using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Dtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using System.Security.Claims;

namespace SastImgAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _dbContext;
        private readonly IValidator<AlbumDto> _albumSetValidator;
        private readonly JwtTokenGenerator _jwtGenerator;
        private readonly CacheAuthAccessor _authAccessor;

        public AlbumController(
            UserManager<User> userManager,
            DatabaseContext dbContext,
            IValidator<AlbumDto> albumSetValidator,
            JwtTokenGenerator jwtGenerator,
            CacheAuthAccessor authAccessor
        )
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _albumSetValidator = albumSetValidator;
            _jwtGenerator = jwtGenerator;
            _authAccessor = authAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Albums(string username, CancellationToken clt)
        {
            var user = await _userManager.Users
                .Include(user => user.Albums)
                .FirstOrDefaultAsync(user => user.NormalizedUserName == username.ToUpper());
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the album.")
                    .Build();
            if (!user.Albums.Any())
            {
                user.Albums.Add(new Album { Name = "Default", Author = user });
                await _dbContext.SaveChangesAsync(clt);
            }
            var albums = user.Albums.Select(
                album =>
                    new
                    {
                        album.Id,
                        album.Name,
                        album.Description,
                        album.CreatedAt,
                        Author = new
                        {
                            album.Author.Id,
                            album.Author.UserName,
                            album.Author.Nickname
                        },
                        album.Accessibility
                    }
            );
            if (User.FindFirstValue("sub") != user.UserName)
                albums = albums.Where(album => album.Accessibility != Accessibility.OnlyMe);
            return ResponseDispatcher.Data(albums);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum(
            [FromBody] AlbumDto albumDto,
            CancellationToken clt
        )
        {
            var validationResult = await _albumSetValidator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();
            var user = (await _userManager.FindByNameAsync(User.FindFirstValue("sub")!))!;

            var album = new Album
            {
                Name = albumDto.Name,
                Description = albumDto.Description,
                Accessibility = albumDto.Accessibility,
                Author = user
            };
            await _dbContext.Albums.AddAsync(album, clt);

            await _dbContext.SaveChangesAsync(clt);
            var createdAlbum = new
            {
                album.Id,
                album.Name,
                album.Description,
                album.CreatedAt,
                Author = new
                {
                    album.Author.Id,
                    album.Author.UserName,
                    album.Author.Nickname
                }
            };
            return ResponseDispatcher.Data(createdAlbum);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ChangeAlbum(
            int id,
            [FromBody] AlbumDto albumDto,
            CancellationToken clt
        )
        {
            var album = await _dbContext.Albums.FirstOrDefaultAsync(album => album.Id == id, clt);
            if (album is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the album.")
                    .Build();
            if (album.AuthorId.ToString() != User.FindFirstValue("id"))
                return Forbid();
            var validationResult = await _albumSetValidator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();
            album.Name = albumDto.Name;
            album.Description = albumDto.Description;
            album.Accessibility = albumDto.Accessibility;

            await _dbContext.SaveChangesAsync(clt);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAlbum(int id, CancellationToken clt)
        {
            int userId = User.Identity!.IsAuthenticated
                ? int.Parse(User.FindFirstValue("id")!)
                : default;

            var album = await _dbContext.Albums
                .Include(album => album.Author)
                .Include(album => album.Images)
                .Select(
                    album =>
                        new
                        {
                            album.Id,
                            album.Name,
                            album.Description,
                            album.CreatedAt,
                            Author = new
                            {
                                album.Author.Id,
                                album.Author.UserName,
                                album.Author.Nickname,
                            },
                            album.Images,
                            album.Accessibility
                        }
                )
                .Where(
                    album =>
                        album.Accessibility == Accessibility.Everyone
                        || album.Author.Id == userId
                        || (
                            User.Identity.IsAuthenticated
                            && album.Accessibility == Accessibility.Auth
                        )
                )
                .FirstOrDefaultAsync(album => album.Id == id);
            if (album is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the album.")
                    .Build();
            else
                return ResponseDispatcher.Data(album);
        }

        //[HttpPost("Auth/{id:int}")]
        //public async Task<IActionResult> AlbumAuth(
        //    int id,
        //    [FromBody] string token,
        //    CancellationToken clt
        //)
        //{
        //    var album = await _dbContext.Albums.FirstOrDefaultAsync(album => album.Id == id, clt);
        //    if (album is null)
        //        return ResponseDispatcher
        //            .Error(StatusCodes.Status404NotFound, "Couldn't find the album.")
        //            .Build();
        //    var passwordHasher = new PasswordHasher<Album>();
        //    if (
        //        album.Accessibility != Accessibility.WithPassword
        //        || User.FindFirstValue("id") == album.AuthorId.ToString()
        //    )
        //        return NoContent();
        //    var result = passwordHasher.VerifyHashedPassword(album, album.PasswordHash!, token);
        //    if (result == PasswordVerificationResult.Failed)
        //        return ResponseDispatcher
        //            .Error(StatusCodes.Status400BadRequest, "Auth failed.")
        //            .Build();
        //    await _authAccessor.SetAuthAsync(User.FindFirstValue("sub")!, id.ToString(), clt);
        //    return NoContent();
        //}

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAlbum(int id, CancellationToken clt)
        {
            var album = await _dbContext.Albums.FirstOrDefaultAsync(album => album.Id == id, clt);
            if (album is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the album.")
                    .Build();
            if (album.AuthorId.ToString() != User.FindFirstValue("id"))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status403Forbidden,
                        "You don't have the access to the album."
                    )
                    .Build();
            _dbContext.Remove(album);
            await _dbContext.SaveChangesAsync(clt);
            return NoContent();
        }
    }
}
