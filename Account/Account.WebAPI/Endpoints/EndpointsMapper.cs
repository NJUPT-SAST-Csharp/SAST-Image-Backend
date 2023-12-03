using Account.Application.Account.Login;
using Account.Application.Account.Register;
using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Mvc;

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

            account
                .MapPost(
                    "/login",
                    ([FromServices] IEndpointHandler<LoginRequest> handler, LoginRequest request) =>
                        handler.Handle(request)
                )
                .AddValidator<LoginRequest>();

            MapRegistration(account);
        }

        private static void MapRegistration(RouteGroupBuilder builder)
        {
            var registration = builder.MapGroup("/registration");

            registration
                .MapPost(
                    "/sendCode",
                    (
                        [FromServices] IEndpointHandler<SendCodeRequest> handler,
                        SendCodeRequest request
                    ) => handler.Handle(request)
                )
                .AddValidator<SendCodeRequest>();
        }
    }
}
