using System.Security.Claims;
using Account.Domain.UserEntity.Commands;
using Account.Domain.UserEntity.ValueObjects;
using Auth;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI;

public static class EndpointsMapper
{
    public static void MapEndpoints(this IEndpointRouteBuilder builder)
    {
        var mapper = builder.MapGroup("/account");

        Login(mapper);
        Register(mapper);
        ChangePassword(mapper);
        Authorize(mapper);
    }

    private static void Authorize(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "/authorize",
                async (
                    [FromBody] AuthorizeRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                    _ = await mediator.Send(
                        new AuthorizeCommand(request.UserId, request.Roles, user),
                        cancellationToken
                    )
            )
            .AddAuthorization(Role.ADMIN)
            .WithSummary("Authorize")
            .WithDescription("Authorize specific user with specific roles.");
    }

    private static void Login(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "/login",
                (
                    [FromBody] LoginRequest request,
                    [FromServices] IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                    mediator.Send(
                        new LoginCommand(request.Username, request.Password),
                        cancellationToken
                    )
            )
            .WithSummary(nameof(Login))
            .WithDescription("Login with username and password.");
    }

    private static void ChangePassword(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(
                "/changePassword",
                async (
                    [FromBody] ChangePasswordRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user,
                    CancellationToken cancellationToken
                ) =>
                    _ = await mediator.Send(
                        new ChangePasswordCommand(request.Password, user),
                        cancellationToken
                    )
            )
            .AddAuthorization(Role.USER)
            .WithSummary("Change Password.")
            .WithDescription("Authorized user changes password.");
    }

    private static void Register(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "/register",
                (
                    [FromBody] RegisterRequest request,
                    [FromServices] IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                    mediator.Send(
                        new RegisterCommand(request.Username, request.Password),
                        cancellationToken
                    )
            )
            .WithSummary("Register and Create Account")
            .WithDescription("Verify registration code and create account with info.");
    }
}

public readonly record struct AuthorizeRequest(UserId UserId, Role[] Roles);

public readonly record struct RegisterRequest(Username Username, PasswordInput Password);

public readonly record struct LoginRequest(Username Username, PasswordInput Password);

public readonly record struct ChangePasswordRequest(PasswordInput Password);
