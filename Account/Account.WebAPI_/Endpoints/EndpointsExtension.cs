namespace Account.WebAPI.Endpoints
{
    internal static class EndpointsExtension
    {
        internal static WebApplication MapEndpoints(this WebApplication app)
        {
            MapUser(app);
            return app;
        }

        private static void MapUser(WebApplication app)
        {
            var userRoute = app.MapGroup("/api/user");
            //userRoute.MapPost(
            //    "/login",
            //    ([FromServices] LoginEndpointHandler handler, LoginRequest request) =>
            //        handler.Handle(request.Username, request.Password)
            //);
            userRoute.MapGet("/test", () => { });
        }
    }
}
