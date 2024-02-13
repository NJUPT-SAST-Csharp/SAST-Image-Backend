using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.UserEndpoints.Query;
using Auth.Authorization;
using Response.Extensions;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsMapper
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api/account").WithOpenApi();

            MapAccount(api);
            MapUser(api);

            return app;
        }

        private static void MapUser(RouteGroupBuilder builder)
        {
            var user = builder.MapGroup("/user");

            user.AddGet<QueryUserRequest>("/", AuthorizationRole.User)
                .WithDataResponse<IEnumerable<QueryUserDto>>()
                .WithUnauthorizedResponse()
                .WithSummary("Query Users")
                .WithDescription("Query specific users by username or ID.");
        }

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder;

            account
                .AddPost<AuthorizeRequest>("/authorize", AuthorizationRole.Admin)
                .WithNoContentResponse()
                .WithUnauthorizedResponse()
                .WithSummary("Authorize user")
                .WithDescription("Authorize specific user with specific role.");

            account
                .AddPost<LoginRequest>("/login")
                .WithDataResponse<LoginDto>()
                .WithSummary("Login")
                .WithDescription("Login with username and password.");

            account
                .AddPut<ChangePasswordRequest>("/changePassword", AuthorizationRole.User)
                .WithNoContentResponse()
                .WithUnauthorizedResponse()
                .WithSummary("Change Password.")
                .WithDescription("Authorized user changes password.");

            MapRegistration(account);
            MapForget(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration
                .AddPost<SendRegistrationCodeRequest>("/sendCode")
                .WithNoContentResponse()
                .WithSummary("Send Registration Code")
                .WithDescription("Send verify code to registrant's email.");

            registration
                .AddPost<VerifyRegistrationCodeRequest>("/verify")
                .WithNoContentResponse()
                .WithSummary("Verify Registration Code")
                .WithDescription("Verify registration code");

            registration
                .AddPost<CreateAccountRequest>("/createAccount")
                .WithNoContentResponse()
                .WithSummary("Register and Create Account")
                .WithDescription("Verify registration code and create account with info.");
        }

        private static void MapForget(RouteGroupBuilder builder)
        {
            var forget = builder.MapGroup("/forget");

            forget
                .AddPost<SendForgetCodeRequest>("/sendCode")
                .WithNoContentResponse()
                .WithSummary("Send ForgetAccount Code")
                .WithDescription("Send code to forgetter's email.");

            forget
                .AddPost<VerifyForgetCodeRequest>("/verify")
                .WithDataResponse<VerifyForgetCodeDto>()
                .WithSummary("Verify ForgetAccount Code")
                .WithDescription("Verify code and return username & ResetCode for account reset.");

            forget
                .AddPost<ResetPasswordRequest>("/reset")
                .WithNoContentResponse()
                .WithSummary("Reset Account for Forgetter")
                .WithDescription("Verify ResetCode and reset account with info.");
        }
    }
}
