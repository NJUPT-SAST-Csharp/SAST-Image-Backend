using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.WebAPI.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Response.ReponseObjects;

namespace Account.WebAPI.Configurations
{
    public static class HttpJsonSerializerExtension
    {
        public static IServiceCollection ConfigureJsonSerializer(this IServiceCollection services)
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            return services;
        }
    }

    [JsonSerializable(typeof(SendRegistrationCodeRequest))]
    [JsonSerializable(typeof(VerifyRegistrationCodeRequest))]
    [JsonSerializable(typeof(CreateAccountRequest))]
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(ChangePasswordRequest))]
    [JsonSerializable(typeof(AuthorizeRequest))]
    [JsonSerializable(typeof(UpdateProfileRequest))]
    [JsonSerializable(typeof(NoContent))]
    [JsonSerializable(typeof(BadRequestResponse))]
    [JsonSerializable(typeof(ProblemDetails))]
    [JsonSerializable(typeof(LoginDto))]
    [JsonSerializable(typeof(VerifyRegistrationCodeDto))]
    [JsonSerializable(typeof(CreateAccountDto))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
