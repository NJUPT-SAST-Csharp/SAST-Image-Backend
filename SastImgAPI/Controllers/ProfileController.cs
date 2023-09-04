using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Response;
using SastImgAPI.Models;
using SastImgAPI.Models.DbSet;
using SastImgAPI.Models.Dtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Services;
using System.Security.Claims;

namespace SastImgAPI.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<ProfileDto> _validator;
        private readonly ImageAccessor _imageAccessor;

        public ProfileController(
            UserManager<User> userManager,
            DatabaseContext dbContext,
            IValidator<ProfileDto> validator,
            ImageAccessor imageAccessor
        )
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _validator = validator;
            _imageAccessor = imageAccessor;
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(
            string username,
            CancellationToken clt,
            [FromQuery] bool detailed = false
        )
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            if (detailed)
                return ResponseDispatcher.Data(
                    new
                    {
                        user.UserName,
                        user.Nickname,
                        user.Email,
                        user.Biography,
                        user.Website,
                        user.Avatar,
                        user.Header,
                        user.RegisteredAt
                    }
                );
            else
                return ResponseDispatcher.Data(
                    new
                    {
                        user.Nickname,
                        user.Biography,
                        user.Avatar,
                    }
                );
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] ProfileDto newProfile,
            CancellationToken clt
        )
        {
            var validationResult = await _validator.ValidateAsync(newProfile, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            var user = await _dbContext.Users.FirstOrDefaultAsync(
                user => user.Id == int.Parse(User.FindFirstValue("id")!),
                clt
            );
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();

            user.Nickname = newProfile.Nickname;
            user.Biography = newProfile.Biography;
            user.Website = newProfile.Website;

            await _dbContext.SaveChangesAsync(clt);

            return NoContent();
        }

        [HttpPost("Avatar")]
        public async Task<IActionResult> UploadAvatar(
            [FromForm] IFormFile avatar,
            CancellationToken clt
        )
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue("id")!);
            var url = await _imageAccessor.UploadAvatarAsync(
                avatar,
                int.Parse(User.FindFirstValue("id")!),
                clt
            );
            user!.Avatar = url;
            _ = _dbContext.SaveChangesAsync(clt);
            return ResponseDispatcher.Data(new { url });
        }

        [HttpPost("Header")]
        public async Task<IActionResult> UploadHeader(
            [FromForm] IFormFile header,
            CancellationToken clt
        )
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue("id")!);
            var url = await _imageAccessor.UploadHeaderAsync(
                header,
                int.Parse(User.FindFirstValue("id")!),
                clt
            );
            user!.Header = url;
            _ = _dbContext.SaveChangesAsync(clt);
            return ResponseDispatcher.Data(new { url });
        }
    }
}
