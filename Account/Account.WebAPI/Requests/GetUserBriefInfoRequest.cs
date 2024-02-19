using Account.Application.UserServices.GetUserBriefInfo;
using Account.WebAPI.SeedWorks;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct GetUserBriefInfoRequest
        : IQueryRequestObject<GetUserBriefInfoQuery, UserBriefInfoDto>
    {
        [FromRoute(Name = "username")]
        public readonly string Username { get; init; }
    }
}
