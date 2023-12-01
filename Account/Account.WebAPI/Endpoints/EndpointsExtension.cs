using Account.Application.Account.Login;
using Account.Application.Account.Register;
using Account.Application.Services;
using Account.WebAPI.Endpoints.Login;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsExtension
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
            account
                .MapPost(
                    "/login",
                    ([FromServices] LoginEndpointHandler handler, LoginRequest request) =>
                        handler.Handle(request)
                )
                .AddEndpointFilter<ValidationFilter<LoginRequest>>();

            var register = account.MapGroup("/register");
            register
                .MapPost(
                    "/sendCode",
                    ([FromServices] SendCodeEndpointHandler handler, SendCodeRequest request) => { }
                )
                .AddEndpointFilter<ValidationFilter<SendCodeRequest>>();
        }
    }
}
