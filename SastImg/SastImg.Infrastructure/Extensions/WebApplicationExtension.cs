using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Primitives.Common.Policies;

namespace SastImg.Infrastructure.Extensions
{
    public static class WebApplicationExtension
    {
        public static void ConfigureApplication(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseRateLimiter();

            app.UseResponseCaching();

            app.MapControllers().RequireRateLimiting(RateLimiterPolicyNames.Concurrency);
        }
    }
}
