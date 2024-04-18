using Auth.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;
using SNS.Application;
using SNS.Application.GetFollowCount;
using SNS.Application.GetFollowers;
using SNS.Application.GetFollowing;
using SNS.Domain.Follows;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SNS.WebAPI.Controllers
{
    [Route("api/sns")]
    [ApiController]
    [Authorize]
    public class FollowController(
        ICommandRequestSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly ICommandRequestSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        [HttpGet("followers")]
        public async Task<IEnumerable<FollowerDto>> GetFollowers([FromQuery] long? id = null)
        {
            return await _querySender.QueryAsync(new GetFollowersQuery(id ?? User.FetchId()));
        }

        [HttpGet("following")]
        public async Task<IEnumerable<FollowingDto>> GetFollowing([FromQuery] long? id = null)
        {
            return await _querySender.QueryAsync(new GetFollowingQuery(id ?? User.FetchId()));
        }

        [HttpPost("follow/{id}")]
        public Task Follow([FromRoute] long id)
        {
            return _commandSender.CommandAsync(
                new FollowCommand(RequesterInfo.FromClaimsPrincipal(User), new(id))
            );
        }

        [HttpDelete("unfollow/{id}")]
        public Task Unfollow([FromRoute] long id)
        {
            return _commandSender.CommandAsync(
                new UnfollowCommand(RequesterInfo.FromClaimsPrincipal(User), new(id))
            );
        }

        [HttpGet("followInfo")]
        public Task<FollowCountDto> GetFollowInfo([FromQuery] long? id = null)
        {
            return _querySender.QueryAsync(new GetFollowCountQuery(id ?? User.FetchId()));
        }
    }
}
