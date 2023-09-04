using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Response;
using SastImgAPI.Models.Dtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Models.Validators;
using SastImgAPI.Options;
using SastImgAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace SastImgAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<PasswordResetDto> _passwordResetValidator;
        private readonly IValidator<EmailConfirmDto> _emailConfirmValidator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailTokenSender _tokenSender;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IDistributedCache _cache;

        public AccountController(
            IValidator<PasswordResetDto> passwordResetValidator,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator,
            UserManager<User> userManager,
            ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            EmailTokenSender tokenSender,
            JwtTokenGenerator jwtTokenGenerator,
            IDistributedCache cache,
            IValidator<EmailConfirmDto> emailConfirmValidator
        )
        {
            _passwordResetValidator = passwordResetValidator;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenSender = tokenSender;
            _jwtTokenGenerator = jwtTokenGenerator;
            _cache = cache;
            _emailConfirmValidator = emailConfirmValidator;
        }

        /// <summary>
        /// Login API.
        /// </summary>
        /// <param name="account">Username and password.</param>
        /// <param name="clt">CancellationToken</param>
        /// <response code="400">Login failed.</response>
        /// <response code="200">Login success, return jwt.</response>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto account, CancellationToken clt)
        {
            var errorResult = ResponseDispatcher
                .Error(StatusCodes.Status400BadRequest, "Username or password incorrect.")
                .Build();

            var validationResult = await _loginValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
                return errorResult;

            var user = await _userManager.FindByNameAsync(account.Username);
            if (user is null)
                return errorResult;

            var loginResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                account.Password,
                false
            );
            if (!loginResult.Succeeded)
                return errorResult;
            var token = await _jwtTokenGenerator.GenerateJwtByUserAsync(user);
            return ResponseDispatcher.Data(new { token });
        }

        [HttpPost]
        public async Task<IActionResult> SendRegisterToken(
            [FromBody] EmailConfirmDto dto,
            CancellationToken clt
        )
        {
            if (!RegexValidator.IsValidEmail(dto.Email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(dto.Email, "Invalid email format.")
                    .Build();

            var check = await _userManager.Users
                .Select(user => user.NormalizedEmail)
                .AnyAsync(email => email == dto.Email.ToUpper(), clt);
            if (check)
                return ResponseDispatcher
                    .Error(StatusCodes.Status409Conflict, "The email has been registered.")
                    .Build();

            string code = CodeGenerator.DefaultCode;
            await _cache.SetStringAsync(
                dto.Email,
                code,
                new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) },
                clt
            );
            _ = _tokenSender.SendEmailTokenAsync(dto.Email, "注册验证", $"您的验证码为{code}，十分钟内有效。");
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateRegisterToken(
            [FromBody] EmailConfirmDto dto,
            CancellationToken clt
        )
        {
            var validationResult = await _emailConfirmValidator.ValidateAsync(dto, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            var token = await _cache.GetStringAsync(dto.Email);
            if (token is null || token != dto.Token)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid token")
                    .Build();
            _ = _cache.RemoveAsync(dto.Email);
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim("email", dto.Email),
                new Claim("role", "Registrant")
            };
            var jwt = _jwtTokenGenerator.GenerateJwtByClaims(claims, TimeSpan.FromMinutes(10));
            return ResponseDispatcher.Data(new { token = jwt });
        }

        /// <summary>
        /// Create a new account.
        /// </summary>
        /// <param name="account">The info of the new account.</param>
        /// <param name="clt">CancellationToken</param>
        /// <response code="400">Register failed.</response>
        /// <response code="200">Register succeeded.</response>
        [HttpPost]
        [Authorize(Roles = "Registrant")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto account,
            CancellationToken clt
        )
        {
            var validationResult = await _registerValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email is null)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status500InternalServerError,
                        "Can't find email claim or invalid email claim."
                    )
                    .Build();

            var user = await _userManager.FindByNameAsync(account.Username);
            if (user is not null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status409Conflict, "The user name has been registered.")
                    .Build();

            User newUser =
                new()
                {
                    UserName = account.Username,
                    Nickname = account.Nickname,
                    Email = email
                };
            var createResult = await _userManager.CreateAsync(newUser, account.Password);
            if (!createResult.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Check Error message for details.")
                    .Add(createResult.Errors)
                    .Build();

            var role = await _roleManager.FindByNameAsync("User");
            if (role is null)
            {
                role = new Role { Name = "User" };
                await _roleManager.CreateAsync(role);
            }
            var result = await _userManager.AddToRoleAsync(newUser, "User");

            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Role add failed")
                    .Add(result.Errors)
                    .Build();

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            newUser.Email = email;

            result = await _userManager.ConfirmEmailAsync(newUser, confirmToken);
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Email confirm failed")
                    .Add(result.Errors)
                    .Build();

            var token = await _jwtTokenGenerator.GenerateJwtByUserAsync(newUser);

            return ResponseDispatcher.Data(new { token });
        }

        [HttpPost]
        public async Task<IActionResult> SendResetToken(
            [FromBody] string email,
            CancellationToken clt
        )
        {
            if (!RegexValidator.IsValidEmail(email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(nameof(email), "Invalid email format.")
                    .Build();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid email address")
                    .Build();
            var token = await _userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultPhoneProvider,
                "PasswordResetValidation"
            );
            var successed = await _tokenSender.SendEmailTokenAsync(
                user.NormalizedEmail!,
                "重置密码",
                $"您的用户名为：{user.UserName}，重置密码的验证码为：{token}"
            );
            if (!successed)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Email send timeout.")
                    .Build();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ValidateResetToken(
            [FromBody] EmailConfirmDto dto,
            CancellationToken clt
        )
        {
            if (!RegexValidator.IsValidEmail(dto.Email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(nameof(dto.Email), "Invalid email format.")
                    .Build();
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();
            var validationResult = await _userManager.VerifyUserTokenAsync(
                user,
                TokenOptions.DefaultPhoneProvider,
                "PasswordResetValidation",
                dto.Token!
            );
            if (!validationResult)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            ICollection<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.UserName!),
                new Claim("token", token),
                new Claim("role", "Resetter")
            };
            var jwt = _jwtTokenGenerator.GenerateJwtByClaims(claims, TimeSpan.FromMinutes(10));

            return ResponseDispatcher.Data(new { token = jwt });
        }

        [Authorize(Roles = "Resetter")]
        [HttpPost]
        public async Task<IActionResult> PasswordReset(
            [FromBody] PasswordResetDto account,
            CancellationToken clt
        )
        {
            var validationResult = await _passwordResetValidator.ValidateAsync(account, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();
            var user = await _userManager.FindByNameAsync(User.FindFirstValue("sub")!);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();

            var result = await _userManager.ResetPasswordAsync(
                user,
                User.FindFirstValue("token")!,
                account.NewPassword
            );
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Add(result.Errors)
                    .Build();

            return NoContent();
        }
    }
}
