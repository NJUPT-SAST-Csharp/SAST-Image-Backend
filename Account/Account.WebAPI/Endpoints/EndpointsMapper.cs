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

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsMapper
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api/account");

            MapAccount(api);
            MapUser(api);

            return app;
        }

        private static void MapUser(RouteGroupBuilder builder)
        {
            var user = builder.MapGroup("/user");

            user.AddGet<QueryUserRequest>("/", AuthorizationRole.User);
        }

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder.MapGroup("/account");

            account.AddPost<AuthorizeRequest>("/authorize", AuthorizationRole.Admin);
            account.AddPost<LoginRequest>("/login");
            account.AddPut<ChangePasswordRequest>("/changePassword", AuthorizationRole.User);

            MapRegistration(account);
            MapForget(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration.AddPost<SendRegistrationCodeRequest>("/sendCode");
            registration.AddPost<VerifyRegistrationCodeRequest>("/verify");
            registration.AddPost<CreateAccountRequest>("/createAccount");
        }

        private static void MapForget(RouteGroupBuilder builder)
        {
            var forget = builder.MapGroup("/forget");

            forget.AddPost<SendForgetCodeRequest>("/sendCode");
            forget.AddPost<VerifyForgetCodeRequest>("/verify");
            forget.AddPost<ResetPasswordRequest>("/reset");
        }
    }
}
