using System.Security.Claims;
using Account.Application.FileServices.GetAvatarFile;
using Account.Application.FileServices.GetHeaderFile;
using Account.Application.FileServices.UpdateAvatar;
using Account.Application.FileServices.UpdateHeader;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.WebAPI.Configurations;
using Account.WebAPI.Requests;
using Account.WebAPI.SeedWorks;
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
        ImageFile(mapper);
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
            .AddValidator<UpdateProfileRequest>()
            .AddAuthorization(Roles.USER)
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
            .AddValidator<GetUserInfoRequest>()
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

    private static void ImageFile(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                "/avatar/{id}",
                async ([FromRoute] long id, [FromServices] IMediator mediator) =>
                {
                    var stream = await mediator.Send(new GetAvatarFileQuery(id));
                    if (stream is null)
                        return Results.NotFound();
                    return Results.File(stream);
                }
            )
            .WithSummary("Get Avatar")
            .WithDescription("Get user avatar image file.");

        builder
            .MapGet(
                "/header/{id}",
                async ([FromRoute] long id, [FromServices] IMediator mediator) =>
                {
                    var stream = await mediator.Send(new GetHeaderFileQuery(id));
                    if (stream is null)
                        return Results.NotFound();
                    return Results.File(stream);
                }
            )
            .WithSummary("Get Header")
            .WithDescription("Get user header image file.");

        builder
            .MapPut(
                "/header",
                (
                    [FromForm] UpdateHeaderRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user
                ) => mediator.Send(new UpdateHeaderCommand(request.HeaderFile, user))
            )
            .DisableAntiforgery()
            .AddValidator<UpdateHeaderRequest>()
            .AddAuthorization(Roles.USER)
            .WithSummary("Update Header")
            .WithDescription("Update user main page's header image.");

        builder
            .MapPut(
                "/avatar",
                (
                    [FromForm] UpdateAvatarRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user
                ) => mediator.Send(new UpdateAvatarCommand(request.AvatarFile, user))
            )
            .DisableAntiforgery()
            .AddValidator<UpdateAvatarRequest>()
            .AddAuthorization(Roles.USER)
            .WithSummary("Update Avatar")
            .WithDescription("Updatet user's avatar image.");
    }
}
