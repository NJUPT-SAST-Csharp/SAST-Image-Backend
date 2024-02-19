using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;
using Account.Application.UserServices.UpdateProfile;
using Account.WebAPI.Configurations;
using Account.WebAPI.Requests;
using Auth.Authorization;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsMapper
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api/account");

            if (app.Environment.IsDevelopment())
            {
                api.WithOpenApi();
            }

            MapAccount(api);
            MapUser(api);

            return app;
        }

        private static void MapUser(RouteGroupBuilder builder)
        {
            var user = builder.MapGroup("/user");

            user.AddPut<UpdateProfileRequest, UpdateProfileCommand>(
                    "/profile",
                    (request, user) => request.ToCommand(user)
                )
                .AddValidator<UpdateProfileRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
                .WithSummary("Update Profile")
                .WithDescription("Update user profile.");

            user.AddGet<GetUserBriefInfoRequest, GetUserBriefInfoQuery, UserBriefInfoDto>(
                    "/{username}",
                    (request, _) => new(request.Username)
                )
                .AddValidator<GetUserBriefInfoRequest>()
                .WithSummary("Query User Brief Info")
                .WithDescription(
                    "Query user brief info, only containing username, nickname, avatar."
                );

            user.AddGet<GetUserDetailedInfoRequest, GetUserDetailedInfoQuery, UserDetailedInfoDto>(
                    "/{username}/detailed",
                    (request, _) => new(request.Username)
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

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder;

            account
                .AddPost<AuthorizeRequest, AuthorizeCommand>(
                    "/authorize",
                    (request, _) => new(request.UserId, request.Roles)
                )
                .AddValidator<AuthorizeRequest>()
                .AddAuthorization(AuthorizationRole.ADMIN)
                .WithSummary("Authorize")
                .WithDescription("Authorize specific user with specific roles.");

            account
                .AddPost<LoginRequest, LoginCommand, LoginDto>(
                    "/login",
                    (request, _) => new(request.Username, request.Password)
                )
                .AddValidator<LoginRequest>()
                .WithSummary("Login")
                .WithDescription("Login with username and password.");

            account
                .AddPut<ChangePasswordRequest, ChangePasswordCommand>(
                    "/changePassword",
                    (request, user) => new(request.NewPassword, user)
                )
                .AddValidator<ChangePasswordRequest>()
                .AddAuthorization(AuthorizationRole.AUTH)
                .WithSummary("Change Password.")
                .WithDescription("Authorized user changes password.");

            MapRegistration(account);
            MapForget(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration
                .AddPost<SendRegistrationCodeRequest, SendRegistrationCodeCommand>(
                    "/sendCode",
                    (request, _) => new(request.Email)
                )
                .AddValidator<SendRegistrationCodeRequest>()
                .WithSummary("Send Registration Code")
                .WithDescription("Send verify code to registrant's email.");

            registration
                .AddPost<
                    VerifyRegistrationCodeRequest,
                    VerifyRegistrationCodeCommand,
                    VerifyRegistrationCodeDto
                >("/verify", (request, _) => new(request.Email, request.Code))
                .AddValidator<VerifyRegistrationCodeRequest>()
                .WithSummary("Verify Registration Code")
                .WithDescription("Verify registration code, only for a snapshot of validation.");

            registration
                .AddPost<CreateAccountRequest, CreateAccountCommand, CreateAccountDto>(
                    "/createAccount",
                    (request, _) =>
                        new(request.Username, request.Password, request.Email, request.Code)
                )
                .AddValidator<CreateAccountRequest>()
                .WithSummary("Register and Create Account")
                .WithDescription("Verify registration code and create account with info.");
        }

        private static void MapForget(RouteGroupBuilder builder)
        {
            var forget = builder.MapGroup("/forget");

            forget
                .AddPost<SendForgetCodeRequest, SendForgetCodeCommand>(
                    "/sendCode",
                    (request, _) => new(request.Email)
                )
                .AddValidator<SendForgetCodeRequest>()
                .WithSummary("Send ForgetAccount Code")
                .WithDescription("Send code to forgetter's email.");

            forget
                .AddPost<VerifyForgetCodeRequest, VerifyForgetCodeCommand, VerifyForgetCodeDto>(
                    "/verify",
                    (request, _) => new(request.Code, request.Email)
                )
                .AddValidator<VerifyForgetCodeRequest>()
                .WithSummary("Verify ForgetAccount Code")
                .WithDescription("Verify code and return username & ResetCode for account reset.");
        }
    }
}
