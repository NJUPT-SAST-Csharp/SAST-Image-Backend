using System.Security.Claims;
using Account.Application.Account.Authorize;
using Account.Application.Account.Login;
using Account.Application.Account.Register.CreateAccount;
using Account.Application.Account.Register.SendCode;
using Account.Application.Account.Register.Verify;
using Auth.Authorization;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsMapper
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");

            api.MapPost("/test", (ClaimsPrincipal user) => user.Identity)
                .RequireAuthorization(AuthorizationRoles.User);

            MapAccount(api);
            return app;
        }

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder.MapGroup("/account");

            account.AddPost<AuthorizeRequest>("/authorize");

            account.AddPost<LoginRequest>("/login");

            MapRegistration(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration.AddPost<SendCodeRequest>("/sendCode");

            registration.AddPost<VerifyRequest>("/verify");

            registration.AddPost<CreateAccountRequest>("/createAccount");
        }
    }
}
