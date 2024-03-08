using System.Security.Claims;
using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.WebAPI.Configurations;
using Account.WebAPI.Requests;
using Account.WebAPI.SeedWorks;
using Auth.Authorization;
using Microsoft.AspNetCore.Mvc;
using Primitives.Command;

namespace Account.WebAPI.Endpoints
{
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
                    (
                        [FromBody] AuthorizeRequest request,
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new AuthorizeCommand(request.UserId, request.Roles)
                        );
                    }
                )
                .AddValidator<AuthorizeRequest>()
                .AddAuthorization(AuthorizationRole.ADMIN)
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
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new LoginCommand(request.Username, request.Password)
                        );
                    }
                )
                .AddValidator<LoginRequest>()
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
                        [FromServices] ICommandRequestSender commandSender,
                        ClaimsPrincipal user
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new ChangePasswordCommand(request.NewPassword, user)
                        );
                    }
                )
                .AddValidator<ChangePasswordRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
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
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new SendRegistrationCodeCommand(request.Email)
                        );
                    }
                )
                .AddValidator<SendRegistrationCodeRequest>()
                .WithSummary("Send Registration Code")
                .WithDescription("Send verify code to registrant's email.");

            mapper
                .MapPost(
                    "/verify",
                    (
                        [FromBody] VerifyRegistrationCodeRequest request,
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new VerifyRegistrationCodeCommand(request.Email, request.Code)
                        );
                    }
                )
                .AddValidator<VerifyRegistrationCodeRequest>()
                .WithSummary("Verify Registration Code")
                .WithDescription("Verify registration code, only for a snapshot of validation.");

            builder
                .MapPost(
                    "/createAccount",
                    (
                        [FromBody] CreateAccountRequest request,
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(request.ToCommand());
                    }
                )
                .AddValidator<CreateAccountRequest>()
                .WithSummary("Register and Create Account")
                .WithDescription("Verify registration code and create account with info.");
        }

        private static void Forget(IEndpointRouteBuilder builder)
        {
            var mapper = builder.MapGroup("/forget");

            mapper
                .MapPost(
                    "/sendCode",
                    (
                        [FromBody] SendForgetCodeRequest request,
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(new SendForgetCodeCommand(request.Email));
                    }
                )
                .AddValidator<SendForgetCodeRequest>()
                .WithSummary("Send ForgetAccount Code")
                .WithDescription("Send code to forgetter's email.");

            mapper
                .MapPost(
                    "/verify",
                    (
                        [FromBody] VerifyForgetCodeRequest request,
                        [FromServices] ICommandRequestSender commandSender
                    ) =>
                    {
                        return commandSender.CommandAsync(
                            new VerifyRegistrationCodeCommand(request.Email, request.Code)
                        );
                    }
                )
                .AddValidator<VerifyForgetCodeRequest>()
                .WithSummary("Verify ForgetAccount Code")
                .WithDescription("Verify code and return username & ResetCode for account reset.");
        }
    }
}
