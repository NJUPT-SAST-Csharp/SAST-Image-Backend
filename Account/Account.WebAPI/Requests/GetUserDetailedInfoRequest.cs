using Account.Application.UserServices.GetUserDetailedInfo;
using Account.WebAPI.SeedWorks;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct GetUserDetailedInfoRequest
        : IQueryRequestObject<GetUserDetailedInfoQuery, UserDetailedInfoDto>
    {
        [FromRoute(Name = "username")]
        public readonly string Username { get; init; }
    }
}
