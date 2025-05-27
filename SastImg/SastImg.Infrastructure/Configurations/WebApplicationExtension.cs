using Auth;
using Microsoft.AspNetCore.Builder;

namespace SastImg.Infrastructure.Configurations;

public static class WebApplicationExtension
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseExceptionHandler(op => { });

        app.UseInternalAuth();

        app.UseResponseCaching();

        app.MapControllers();
    }
}
