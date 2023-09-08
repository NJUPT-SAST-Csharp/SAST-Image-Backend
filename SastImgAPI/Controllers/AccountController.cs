using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Response;
using SastImgAPI.Models.RequestDtos;
using SastImgAPI.Models.Identity;
using SastImgAPI.Models.Validators;
using SastImgAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using SastImgAPI.Models.ResponseDtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SastImgAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<PasswordResetRequestDto> _passwordResetValidator;
        private readonly IValidator<EmailConfirmRequestDto> _emailConfirmValidator;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailTokenSender _tokenSender;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IDistributedCache _cache;

        public AccountController(
            IValidator<PasswordResetRequestDto> passwordResetValidator,
            IValidator<RegisterRequestDto> registerValidator,
            IValidator<LoginRequestDto> loginValidator,
            UserManager<User> userManager,
            ILogger<AccountController> logger,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            EmailTokenSender tokenSender,
            JwtTokenGenerator jwtTokenGenerator,
            IDistributedCache cache,
            IValidator<EmailConfirmRequestDto> emailConfirmValidator
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
        /// User login.
        /// </summary>
        /// <remarks>
        /// Allows users to login by providing valid credentials.
        /// If the provided credentials are valid, a JWT token is generated and returned for authentication.
        /// </remarks>
        /// <param name="account">An object containing login credentials.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns a JWT token.",
            typeof(JwtResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: Username or password incorrect or validation failed.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto account,
            CancellationToken clt
        )
        {
            // Build an error result for login failure
            var errorResult = ResponseDispatcher
                .Error(StatusCodes.Status400BadRequest, "Username or password incorrect.")
                .Build();

            // Validate login credentials
            var validationResult = await _loginValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
                return errorResult;

            // Find the user
            var user = await _userManager.FindByNameAsync(account.Username);
            if (user is null)
                return errorResult;

            // Check the password and sign in
            var loginResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                account.Password,
                false
            );
            if (!loginResult.Succeeded)
                return errorResult;

            // Generate a JWT token and return a successful result
            var token = await _jwtTokenGenerator.GenerateJwtByUserAsync(user);
            return ResponseDispatcher.Data(new JwtResponseDto(token));
        }

        /// <summary>
        /// Send a registration confirmation token to the specified email address.
        /// </summary>
        /// <remarks>
        /// Send a registration confirmation token to the provided email address.
        /// Upon successful token generation, the token is sent via email, and the user can use it to complete the registration process.
        /// The token is stored temporarily in a cache and can be used only once for validation.
        /// </remarks>
        /// <param name="dto">An object containing email information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The registration confirmation token has been sent successfully."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status409Conflict,
            "Conflict: The email address has already been registered.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> SendRegisterToken(
            [FromBody] EmailConfirmRequestDto dto,
            CancellationToken clt
        )
        {
            // Check if the provided email is in a valid format
            if (!RegexValidator.IsValidEmail(dto.Email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters to your request was invalid."
                    )
                    .Add(dto.Email, "Invalid email format.")
                    .Build();

            // Check if the email is already registered
            var check = await _userManager.Users
                .Select(user => user.NormalizedEmail)
                .AnyAsync(email => email == dto.Email.ToUpper(), clt);
            if (check)
                return ResponseDispatcher
                    .Error(StatusCodes.Status409Conflict, "The email has been registered.")
                    .Build();

            // Generate a confirmation code and store it in a cache
            string code = CodeGenerator.DefaultCode;
            await _cache.SetStringAsync(
                dto.Email,
                code,
                new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) },
                clt
            );

            // Send the confirmation token email
            _ = _tokenSender.SendEmailTokenAsync(
                dto.Email,
                "Registration Confirmation",
                $"Your code is {code}, valid for ten minutes."
            );

            // Return a successful response with no content
            return NoContent();
        }

        /// <summary>
        /// Validate a registration confirmation token sent to the specified email address.
        /// </summary>
        /// <remarks>
        /// Validate the registration confirmation token received via email.
        /// If the token is valid, a JWT token with "Registrant" role is generated and returned, allowing the user to complete the registration process.
        /// </remarks>
        /// <param name="dto">An object containing email and token information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns a JWT token with role \"Registrant\" for registration.",
            typeof(JwtResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid or the token is invalid.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> ValidateRegisterToken(
            [FromBody] EmailConfirmRequestDto dto,
            CancellationToken clt
        )
        {
            // Validate the registration confirmation token
            var validationResult = await _emailConfirmValidator.ValidateAsync(dto, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request was invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Check if the provided token matches the one stored in the cache
            var token = await _cache.GetStringAsync(dto.Email);
            if (token is null || token != dto.Token)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid token")
                    .Build();

            // Remove the token from the cache (used for one-time validation)
            _ = _cache.RemoveAsync(dto.Email);

            // Generate a JWT token for registration and return it
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim("email", dto.Email),
                new Claim("role", "Registrant")
            };
            var jwt = _jwtTokenGenerator.GenerateJwtByClaims(claims, TimeSpan.FromMinutes(10));
            return ResponseDispatcher.Data(new JwtResponseDto(jwt));
        }

        /// <summary>
        /// User registration.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Registrant" role to register for the application.
        /// Upon successful registration, a JWT token is generated and returned for authentication.
        /// </remarks>
        /// <param name="account">An object containing registration information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [Authorize(Roles = "Registrant")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns a JWT token.",
            typeof(JwtResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid or registration fails.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status409Conflict,
            "Conflict: The requested username is already registered.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal Server Error: An error occurred during registration.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto account,
            CancellationToken clt
        )
        {
            // Validate the registration data
            var validationResult = await _registerValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request were invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Retrieve the user's email from the claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email is null)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status500InternalServerError,
                        "Cannot find email claim or invalid email claim."
                    )
                    .Build();

            // Check if the username is already registered
            var user = await _userManager.FindByNameAsync(account.Username);
            if (user is not null)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status409Conflict,
                        "The username has already been registered."
                    )
                    .Build();

            // Create a new user with registration data
            User newUser =
                new()
                {
                    UserName = account.Username,
                    Nickname = account.Nickname,
                    Email = email
                };

            // Attempt to create the user
            var createResult = await _userManager.CreateAsync(newUser, account.Password);
            if (!createResult.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Check Error message for details.")
                    .Add(createResult.Errors)
                    .Build();

            // Ensure the 'User' role exists and add the user to it
            var role = await _roleManager.FindByNameAsync("User");
            if (role is null)
            {
                role = new Role { Name = "User" };
                await _roleManager.CreateAsync(role);
            }

            // Add the user to the 'User' role
            var result = await _userManager.AddToRoleAsync(newUser, "User");

            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Role assignment failed.")
                    .Add(result.Errors)
                    .Build();

            // Generate an email confirmation token
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            // Set the user's email
            newUser.Email = email;

            // Confirm the user's email
            result = await _userManager.ConfirmEmailAsync(newUser, confirmToken);
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Email confirmation failed.")
                    .Add(result.Errors)
                    .Build();

            // Generate a JWT token for the newly registered user and return it
            var token = await _jwtTokenGenerator.GenerateJwtByUserAsync(newUser);
            return ResponseDispatcher.Data(new JwtResponseDto(token));
        }

        /// <summary>
        /// Send a password reset token to the specified email address.
        /// </summary>
        /// <remarks>
        /// Send a password reset token to the provided email address.
        /// Upon successful token generation, the token is sent via email to allow the user to reset their password.
        /// The token is generated using the specified token provider.
        /// </remarks>
        /// <param name="email">The email address to which the password reset token will be sent.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: The password reset token has been sent successfully."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: The email parameter is invalid or the email address does not exist.",
            typeof(ErrorResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status500InternalServerError,
            "Internal Server Error: An error occurred while sending the email.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> SendResetToken(
            [FromBody] string email,
            CancellationToken clt
        )
        {
            // Validate the email format
            if (!RegexValidator.IsValidEmail(email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request are invalid."
                    )
                    .Add(nameof(email), "Invalid email format.")
                    .Build();

            // Find the user by their email address
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid email address")
                    .Build();

            // Generate a password reset token
            var token = await _userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultPhoneProvider,
                "PasswordResetValidation"
            );

            // Send the password reset token via email
            var successed = await _tokenSender.SendEmailTokenAsync(
                user.NormalizedEmail!,
                "重置密码",
                $"您的用户名为：{user.UserName}，重置密码的验证码为：{token}"
            );
            // Handle email send success or failure
            if (!successed)
                return ResponseDispatcher
                    .Error(StatusCodes.Status500InternalServerError, "Email send timeout.")
                    .Build();

            // Return a successful response with no content
            return NoContent();
        }

        /// <summary>
        /// Validate a password reset token and generating a JWT token for reset process.
        /// </summary>
        /// <remarks>
        /// Validate a password reset token received via email and generate a new JWT token.
        /// Users who have initiated a password reset process can use this endpoint to confirm their token and receive a new JWT token for authentication.
        /// After successful token validation, a new password reset token is generated for the user.
        /// </remarks>
        /// <param name="dto">An object containing email and token information.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Ok: Returns a JWT token with role \"Resetter\" for reset process.",
            typeof(JwtResponseDto)
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid or token is invalid.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> ValidateResetToken(
            [FromBody] EmailConfirmRequestDto dto,
            CancellationToken clt
        )
        {
            // Validate the email format
            if (!RegexValidator.IsValidEmail(dto.Email))
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request are invalid."
                    )
                    .Add(nameof(dto.Email), "Invalid email format.")
                    .Build();

            // Find the user by their email address
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();

            // Verify the user's password reset token
            var validationResult = await _userManager.VerifyUserTokenAsync(
                user,
                TokenOptions.DefaultPhoneProvider,
                "PasswordResetValidation",
                dto.Token!
            );

            // Handle token validation success or failure
            if (!validationResult)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();

            // Generate a new password reset token for the user
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Generate a new JWT token containing user claims
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim("sub", user.UserName!),
                new Claim("token", token),
                new Claim("role", "Resetter")
            };
            var jwt = _jwtTokenGenerator.GenerateJwtByClaims(claims, TimeSpan.FromMinutes(10));

            // Return a successful response with the new JWT token
            return ResponseDispatcher.Data(new JwtResponseDto(jwt));
        }

        /// <summary>
        /// Reset a user's password.
        /// </summary>
        /// <remarks>
        /// Allow users with the "Resetter" role to reset their password.
        /// Users must provide a valid password reset token and a new password to complete the reset process.
        /// Upon successful password reset, the user's password is updated.
        /// </remarks>
        /// <param name="account">Dto object containing reset credentials.</param>
        /// <param name="clt">A CancellationToken used for canceling the operation.</param>
        [HttpPost]
        [Authorize(Roles = "Resetter")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "No Content: Password reset successful. The password has been updated."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Bad Request: One or more parameters in your request are invalid.",
            typeof(ErrorResponseDto)
        )]
        public async Task<IActionResult> PasswordReset(
            [FromBody] PasswordResetRequestDto account,
            CancellationToken clt
        )
        {
            // Validate the reset credentials
            var validationResult = await _passwordResetValidator.ValidateAsync(account, clt);
            if (!validationResult.IsValid)
                return ResponseDispatcher
                    .Error(
                        StatusCodes.Status400BadRequest,
                        "One or more parameters in your request are invalid."
                    )
                    .Add(validationResult.Errors)
                    .Build();

            // Find the user by their username
            var user = await _userManager.FindByNameAsync(User.FindFirstValue("sub")!);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Build();

            // Reset the user's password using the provided reset token and new password
            var result = await _userManager.ResetPasswordAsync(
                user,
                User.FindFirstValue("token")!,
                account.NewPassword
            );

            // Handle password reset success or failure
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Invalid request")
                    .Add(result.Errors)
                    .Build();

            // Return a successful response with no content
            return NoContent();
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetRole(
            [FromBody] RoleSetRequestDto data,
            CancellationToken clt
        )
        {
            var user = await _userManager.FindByNameAsync(data.Username);
            var role = await _roleManager.FindByNameAsync(data.RoleName);
            if (user is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific user.")
                    .Build();
            if (role is null)
                return ResponseDispatcher
                    .Error(StatusCodes.Status404NotFound, "Couldn't find the specific role.")
                    .Build();
            var result = await _userManager.AddToRoleAsync(user, data.RoleName);
            if (!result.Succeeded)
                return ResponseDispatcher
                    .Error(StatusCodes.Status400BadRequest, "Add failed.")
                    .Build();
            return NoContent();
        }
    }
}
