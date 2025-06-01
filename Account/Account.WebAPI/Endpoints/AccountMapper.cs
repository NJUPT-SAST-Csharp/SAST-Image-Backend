using System.Security.Claims;
using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.WebAPI.Requests;
using Account.WebAPI.SeedWorks;
using Auth;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Endpoints;

public sealed class AccountMapper : IEndpointMapper
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var mapper = builder.MapGroup("/account");

        Login(mapper);
        Register(mapper);
        ChangePassword(mapper);
        Authorize(mapper);
        Forget(mapper);
    }

    private static void Authorize(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "/authorize",
                ([FromBody] AuthorizeRequest request, [FromServices] IMediator mediator) =>
                {
                    return mediator.Send(new AuthorizeCommand(request.UserId, request.Roles));
                }
            )
            .AddAuthorization(Roles.ADMIN)
            .WithSummary("Authorize")
            .WithDescription("Authorize specific user with specific roles.");
    }

    private static void Login(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "/login",
                ([FromBody] LoginRequest request, [FromServices] IMediator mediator) =>
                {
                    return mediator.Send(new LoginCommand(request.Username, request.Password));
                }
            )
            .WithSummary(nameof(Login))
            .WithDescription("Login with username and password.");
    }

    private static void ChangePassword(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(
                "/changePassword",
                (
                    [FromBody] ChangePasswordRequest request,
                    [FromServices] IMediator mediator,
                    ClaimsPrincipal user
                ) =>
                {
                    return mediator.Send(new ChangePasswordCommand(request.NewPassword, user));
                }
            )
            .AddAuthorization(Roles.USER)
            .WithSummary("Change Password.")
            .WithDescription("Authorized user changes password.");
    }

    private static void Register(IEndpointRouteBuilder builder)
    {
        var mapper = builder.MapGroup("/registration");

        mapper
            .MapPost(
                "/sendCode",
                (
                    [FromBody] SendRegistrationCodeRequest request,
                    [FromServices] IMediator mediator
                ) =>
                {
                    return mediator.Send(new SendRegistrationCodeCommand(request.Email));
                }
            )
            .WithSummary("Send Registration Code")
            .WithDescription("Send verify code to registrant's email.");

        mapper
            .MapPost(
                "/verify",
                (
                    [FromBody] VerifyRegistrationCodeRequest request,
                    [FromServices] IMediator mediator
                ) =>
                {
                    return mediator.Send(
                        new VerifyRegistrationCodeCommand(request.Email, request.Code)
                    );
                }
            )
            .WithSummary("Verify Registration Code")
            .WithDescription("Verify registration code, only for a snapshot of validation.");

        builder
            .MapPost(
                "/createAccount",
                ([FromBody] CreateAccountRequest request, [FromServices] IMediator mediator) =>
                {
                    return mediator.Send(request.ToCommand());
                }
            )
            .WithSummary("Register and Create Account")
            .WithDescription("Verify registration code and create account with info.");
    }

    private static void Forget(IEndpointRouteBuilder builder)
    {
        var mapper = builder.MapGroup("/forget");

        mapper
            .MapPost(
                "/sendCode",
                ([FromBody] SendForgetCodeRequest request, [FromServices] IMediator mediator) =>
                {
                    return mediator.Send(new SendForgetCodeCommand(request.Email));
                }
            )
            .WithSummary("Send ForgetAccount Code")
            .WithDescription("Send code to forgetter's email.");

        mapper
            .MapPost(
                "/verify",
                ([FromBody] VerifyForgetCodeRequest request, [FromServices] IMediator mediator) =>
                {
                    return mediator.Send(
                        new VerifyRegistrationCodeCommand(request.Email, request.Code)
                    );
                }
            )
            .WithSummary("Verify ForgetAccount Code")
            .WithDescription("Verify code and return username & ResetCode for account reset.");
    }
}
