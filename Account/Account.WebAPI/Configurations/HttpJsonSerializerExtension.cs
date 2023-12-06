using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Application.Account.Login;
using Account.Application.Account.Register.SendCode;
using Account.Application.Account.Register.Verify;
using Response.ReponseObjects;

namespace Account.WebAPI.Configurations
{
    public static class HttpJsonSerializerExtension
    {
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
            });
            return services;
        }
    }

    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(VerifyRequest))]
    [JsonSerializable(typeof(SendCodeRequest))]
    [JsonSerializable(typeof(BadRequestResponse))]
    [JsonSerializable(typeof(DataResponse<LoginDto>))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
