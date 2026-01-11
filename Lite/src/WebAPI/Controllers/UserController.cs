using System.ComponentModel.DataAnnotations;
using Application.UserServices.Queries;
using Domain.UserAggregate.Commands;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers;

[Route("api/users")]
[ApiController]
public sealed class UserController(IMediator mediator) : ControllerBase
{
    #region [Command/Post]


    public readonly record struct UpdateNicknameRequest(Nickname Nickname);

    [Authorize]
    [HttpPost("nickname")]
    public async Task<IActionResult> UpdateNickname(
        [FromBody] [Required] UpdateNicknameRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateNicknameCommand command = new(request.Nickname, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public readonly record struct UpdateBiographyRequest(Biography Biography);

    [Authorize]
    [HttpPost("biography")]
    public async Task<IActionResult> UpdateBiography(
        [FromBody] [Required] UpdateBiographyRequest request,
        CancellationToken cancellationToken
    )
    {
        UpdateBiographyCommand command = new(request.Biography, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UpdateAvatar(
        [FromForm] [FileValidator(0, 3)] [Required] IFormFile avatar,
        CancellationToken cancellationToken
    )
    {
        using var file = ImageFile.Create(avatar.OpenReadStream());

        UpdateAvatarCommand command = new(file, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("header")]
    public async Task<IActionResult> UpdateHeader(
        [FromForm] [FileValidator(0, 10)] [Required] IFormFile header,
        CancellationToken cancellationToken
    )
    {
        using var file = ImageFile.Create(header.OpenReadStream());

        UpdateHeaderCommand command = new(file, User);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    #endregion

    #region [Query/Get]

    [HttpGet("{id:long}/avatar")]
    public async Task<IActionResult> GetAvatar(
        [FromRoute] UserId id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserAvatarQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return this.AvatarOrNotFound(result);
    }

    [HttpGet("{id:long}/header")]
    public async Task<IActionResult> GetHeader(
        [FromRoute] UserId id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserHeaderQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return this.HeaderOrNotFound(result);
    }

    [HttpGet("{id:long}/profile")]
    public async Task<IActionResult> GetProfileInfo(
        [FromRoute] UserId id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserProfileQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return this.DataOrNotFound(result);
    }

    #endregion
}
