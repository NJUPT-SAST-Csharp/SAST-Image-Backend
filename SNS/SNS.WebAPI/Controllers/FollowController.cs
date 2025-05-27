using Identity;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNS.Application.GetFollowCount;
using SNS.Application.GetFollowers;
using SNS.Application.GetFollowing;
using SNS.Domain.Follows;

namespace SNS.WebAPI.Controllers;

[Route("api/sns")]
[ApiController]
[Authorize]
public class FollowController(IMediator mediator) : ControllerBase
{
    [HttpGet("followers")]
    public async Task<IEnumerable<FollowerDto>> GetFollowers([FromQuery] long? id = null) =>
        await mediator.Send(new GetFollowersQuery(id ?? new Requester(User).Id.Value));

    [HttpGet("following")]
    public async Task<IEnumerable<FollowingDto>> GetFollowing([FromQuery] long? id = null) =>
        await mediator.Send(new GetFollowingQuery(id ?? new Requester(User).Id.Value));

    [HttpGet("followInfo")]
    public async Task<FollowCountDto> GetFollowInfo([FromQuery] long? id = null) =>
        await mediator.Send(new GetFollowCountQuery(id ?? new Requester(User).Id.Value));

    [HttpPost("follow/{id}")]
    public async Task Follow([FromRoute] long id) =>
        await mediator.Send(new FollowCommand(new Requester(User).Id, new(id)));

    [HttpDelete("unfollow/{id}")]
    public async Task Unfollow([FromRoute] long id) =>
        await mediator.Send(new UnfollowCommand(new Requester(User).Id, new(id)));
}
