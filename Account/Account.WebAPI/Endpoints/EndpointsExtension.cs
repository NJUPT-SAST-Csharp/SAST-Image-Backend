using Account.Application.Users.Login;
using Account.WebAPI.Endpoints.Login;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsExtension
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");
            MapUser(api);
            return app;
        }

        private static void MapUser(RouteGroupBuilder builder)
        {
            var userRoute = builder.MapGroup("/user");
            userRoute.MapPost(
                "/login",
                ([FromServices] LoginEndpointHandler handler, LoginRequest request) =>
                {
                    return handler.Handle(request);
                }
            );
            userRoute.MapGet("/test", () => { });
        }
    }
}
