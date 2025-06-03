using System.Security.Claims;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.WebAPI.Requests;
using Account.WebAPI.SeedWorks;
using Auth;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Endpoints;

public sealed class ProfileMapper : IEndpointMapper
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var mapper = builder.MapGroup("/profile");

        ProfileInfo(mapper);
    }

    private static void ProfileInfo(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(
                "/",
                (
                    [FromBody] UpdateProfileRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user
                ) =>
                {
                    return mediator.Send(request.ToCommand(user));
                }
            )
            .AddAuthorization(Role.USER)
            .WithSummary("Update Profile")
            .WithDescription("Update user profile.");

        builder
            .MapGet(
                "/",
                ([AsParameters] GetUserInfoRequest request, [FromServices] IMediator mediator) =>
                    mediator.Send(
                        new GetUserInfoQuery(request.Username, request.UserId, request.IsDetailed)
                    )
            )
            .WithSummary("Query User Info")
            .WithDescription(
                """
                Query user brief info, only containing username, nickname, avatar.

                Query user detailed info by adding query param 'detailed', 
                containing username, nickname, avatar, header, bio, birthday, website.

                id has a higher priority
                """
            );
    }
}
