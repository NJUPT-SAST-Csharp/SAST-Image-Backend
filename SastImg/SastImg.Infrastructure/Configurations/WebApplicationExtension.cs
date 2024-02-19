using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace SastImg.Infrastructure.Configurations
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

            app.UseExceptionHandler(op => { });

            app.UseAuthorization();

            app.UseResponseCaching();

            app.MapControllers();
        }
    }
}
