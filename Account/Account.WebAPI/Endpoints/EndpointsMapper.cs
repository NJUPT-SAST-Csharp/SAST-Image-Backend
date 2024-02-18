using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.WebAPI.Configurations;
using Account.WebAPI.Requests;
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

            //user.AddGet<QueryUserRequest>("/", AuthorizationRole.USER)
            //    .WithDataResponse<IEnumerable<QueryUserDto>>()
            //    .WithUnauthorizedResponse()
            //    .WithSummary("Query Users")
            //    .WithDescription("Query specific users by username or ID.");
        }

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder;

            account.MapPost("/authorize", () => { });

            account
                .AddPost<AuthorizeRequest, AuthorizeCommand>(
                    "/authorize",
                    (request, _) => new AuthorizeCommand(request.UserId, request.RoleId)
                )
                .WithNoContentResponse()
                .WithUnauthorizedResponse()
                .WithSummary("Authorize user")
                .WithDescription("Authorize specific user with specific role.");

            //account
            //    .AddPost<AuthorizeCommand>("/authorize", AuthorizationRole.ADMIN)
            //    .WithNoContentResponse()
            //    .WithUnauthorizedResponse()
            //    .WithSummary("Authorize user")
            //    .WithDescription("Authorize specific user with specific role.");

            //account
            //    .AddPost<LoginCommand>("/login")
            //    .WithDataResponse<LoginDto>()
            //    .WithSummary("LoginAsync")
            //    .WithDescription("LoginAsync with username and password.");

            //account
            //    .AddPut<ChangePasswordCommand>("/changePassword", AuthorizationRole.USER)
            //    .WithNoContentResponse()
            //    .WithUnauthorizedResponse()
            //    .WithSummary("Change Password.")
            //    .WithDescription("Authorized user changes password.");

            MapRegistration(account);
            MapForget(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            //registration
            //    .AddPost<SendRegistrationCodeCommand>("/sendCode")
            //    .WithNoContentResponse()
            //    .WithSummary("Send Registration Code")
            //    .WithDescription("Send verify code to registrant's email.");

            //registration
            //    .AddPost<VerifyRegistrationCodeCommand>("/verify")
            //    .WithNoContentResponse()
            //    .WithSummary("Verify Registration Code")
            //    .WithDescription("Verify registration code");

            //registration
            //    .AddPost<CreateAccountCommand>("/createAccount")
            //    .WithDataResponse<CreateAccountDto>()
            //    .WithSummary("Register and Create Account")
            //    .WithDescription("Verify registration code and create account with info.");
        }

        private static void MapForget(RouteGroupBuilder builder)
        {
            var forget = builder.MapGroup("/forget");

            //forget
            //    .AddPost<SendForgetCodeCommand>("/sendCode")
            //    .WithNoContentResponse()
            //    .WithSummary("Send ForgetAccount Code")
            //    .WithDescription("Send code to forgetter's email.");

            //forget
            //    .AddPost<VerifyForgetCodeCommand>("/verify")
            //    .WithDataResponse<VerifyForgetCodeDto>()
            //    .WithSummary("Verify ForgetAccount Code")
            //    .WithDescription("Verify code and return username & ResetCode for account reset.");

            //forget
            //    .AddPost<ResetPasswordRequest>("/reset")
            //    .WithNoContentResponse()
            //    .WithSummary("Reset Account for Forgetter")
            //    .WithDescription("Verify ResetCode and reset account with info.");
        }
    }
}
