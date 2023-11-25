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
            var userRoute = app.MapGroup("/user");
            userRoute.MapGet("/login", () => { });
        }
    }
}
