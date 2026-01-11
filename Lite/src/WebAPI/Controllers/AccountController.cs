using System.ComponentModel.DataAnnotations;
using Application.UserServices.Queries;
using Domain.UserAggregate.Commands;
using Domain.UserAggregate.UserEntity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers;

[Route("api/account")]
[ApiController]
public sealed class AccountController(IMediator mediator) : ControllerBase
{
    public readonly record struct RegisterRequest(
        Username Username,
        Nickname Nickname,
        PasswordInput Password,
        RegistryCode Code
    );

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] [Required] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        RegisterCommand command = new(
            request.Username,
            request.Nickname,
            request.Password,
            request.Code
        );
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    public readonly record struct LoginRequest(Username Username, PasswordInput Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] [Required] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        LoginCommand command = new(request.Username, request.Password);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    public readonly record struct RefreshTokenRequest(RefreshToken RefreshToken);

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAccessToken(
        [FromBody] [Required] RefreshTokenRequest request,
        CancellationToken cancellationToken
    )
    {
        RefreshTokenCommand command = new(request.RefreshToken);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    public readonly record struct ResetPasswordRequest(
        PasswordInput OldPassword,
        PasswordInput NewPassword
    );

    [Authorize]
    [HttpPost("reset/password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] [Required] ResetPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        ResetPasswordCommand command = new(request.OldPassword, request.NewPassword, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct ResetUsernameRequest(Username Username);

    [Authorize]
    [HttpPost("reset/username")]
    public async Task<IActionResult> ResetUsername(
        [FromBody] [Required] ResetUsernameRequest request,
        CancellationToken cancellationToken
    )
    {
        ResetUsernameCommand command = new(request.Username, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("username/check")]
    public async Task<IActionResult> CheckUsername(
        [FromQuery] [Required] [Length(Username.MinLength, Username.MaxLength)] string username,
        CancellationToken cancellationToken
    )
    {
        var query = new UsernameExistenceQuery(username.Bind<Username>());
        var result = await mediator.Send(query, cancellationToken);
        return Ok(!result.IsExist);
    }
}
