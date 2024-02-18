using System.Text.Json;
using System.Text.Json.Serialization;
using Account.WebAPI.Requests;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Account.WebAPI.Configurations
{
    /// <summary>
    /// TODO: complete
    /// </summary>
    public static class HttpJsonSerializerExtension
    {
        /// <summary>
        /// TODO: complete
        /// </summary>
        public static IServiceCollection ConfigureJsonSerializer(this IServiceCollection services)
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                //options
                //    .SerializerOptions
                //    .TypeInfoResolverChain
                //    .Insert(0, AppJsonSerializerContext.Default);
                options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            return services;
        }
    }

    [JsonSerializable(typeof(AuthorizeRequest))]
    [JsonSerializable(typeof(NoContent))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
