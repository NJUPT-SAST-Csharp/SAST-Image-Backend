using System.Security.Claims;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;
using Account.Application.UserServices.UpdateAvatar;
using Account.Application.UserServices.UpdateHeader;
using Account.WebAPI.Configurations;
using Account.WebAPI.Requests;
using Account.WebAPI.SeedWorks;
using Auth.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;
using Primitives.Query;

namespace Account.WebAPI.Endpoints
{
    public sealed class ProfileMapper : IEndpointMapper
    {
        public void MapEndpoints(IEndpointRouteBuilder builder)
        {
            var mapper = builder.MapGroup("/profile");

            UpdateProfile(mapper);
            GetProfile(mapper);
        }

        private static void UpdateProfile(IEndpointRouteBuilder builder)
        {
            builder
                .MapPut(
                    "/",
                    (
                        [FromBody] UpdateProfileRequest request,
                        [FromServices] ICommandRequestSender commandSender,
                        ClaimsPrincipal user
                    ) =>
                    {
                        return commandSender.CommandAsync(request.ToCommand(user));
                    }
                )
                .AddValidator<UpdateProfileRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
                .WithSummary("Update Profile")
                .WithDescription("Update user profile.");

            builder
                .MapPut(
                    "/header",
                    (
                        [FromForm] UpdateHeaderRequest request,
                        [FromServices] ICommandRequestSender commandSender,
                        ClaimsPrincipal user
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new UpdateHeaderCommand(request.HeaderFile, user)
                        );
                    }
                )
                .DisableAntiforgery()
                .AddValidator<UpdateHeaderRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
                .WithSummary("Update Header")
                .WithDescription("Update user main page's header image.");

            builder
                .MapPut(
                    "/avatar",
                    (
                        [FromForm] UpdateAvatarRequest request,
                        [FromServices] ICommandRequestSender commandSender,
                        ClaimsPrincipal user
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new UpdateAvatarCommand(request.AvatarFile, user)
                        );
                    }
                )
                .DisableAntiforgery()
                .AddValidator<UpdateAvatarRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
                .WithSummary("Update Avatar")
                .WithDescription("Updatet user's avatar image.");
        }

        private static void GetProfile(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet(
                    "/{username}",
                    (
                        [AsParameters] GetUserBriefInfoRequest request,
                        [FromServices] IQueryRequestSender querySender
                    ) =>
                    {
                        return querySender.QueryAsync(new GetUserBriefInfoQuery(request.Username));
                    }
                )
                .AddValidator<GetUserBriefInfoRequest>()
                .WithSummary("Query User Brief Info")
                .WithDescription(
                    "Query user brief info, only containing username, nickname, avatar."
                );

            builder
                .MapGet(
                    "/{username}/detailed",
                    (
                        [AsParameters] GetUserDetailedInfoRequest request,
                        [FromServices] IQueryRequestSender querySender
                    ) =>
                    {
                        return querySender.QueryAsync(
                            new GetUserDetailedInfoQuery(request.Username)
                        );
                    }
                )
                .AddAuthorization(AuthorizationRole.AUTH)
                .AddValidator<GetUserDetailedInfoRequest>()
                .WithSummary("Query User Detailed Info")
                .WithDescription(
                    """
                    Query user detailed info, 
                    containing username, nickname, avatar, header, bio, birthday, website
                    """
                );
        }
    }
}
