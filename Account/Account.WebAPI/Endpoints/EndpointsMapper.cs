using Account.Application.Account.Login;
using Account.Application.Account.Register.SendCode;
using Account.Application.Account.Register.Verify;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsMapper
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");
            MapAccount(api);
            return app;
        }

        private static void MapAccount(RouteGroupBuilder builder)
        {
            var account = builder.MapGroup("/account");

            account.AddPost<LoginRequest>("/login");

            MapRegistration(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration.AddPost<SendCodeRequest>("/sendCode");

            registration.AddPost<VerifyRequest>("/verify");

            registration.AddPost<VerifyRequest>("/createAccount");
        }
    }
}
