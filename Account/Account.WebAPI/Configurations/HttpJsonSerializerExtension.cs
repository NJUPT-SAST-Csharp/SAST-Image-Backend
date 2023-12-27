using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Application.Endpoints.AccountEndpoints.ChangePassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.ResetPassword;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;
using Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;
using Response.ReponseObjects;

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

    /// <summary>
    /// TODO: complete
    /// </summary>
    [JsonSerializable(typeof(ClaimsPrincipal))]
    [JsonSerializable(typeof(BadRequestResponse))]
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(AuthorizeRequest))]
    [JsonSerializable(typeof(VerifyRegistrationCodeRequest))]
    [JsonSerializable(typeof(ResetPasswordRequest))]
    [JsonSerializable(typeof(VerifyForgetCodeRequest))]
    [JsonSerializable(typeof(SendForgetCodeRequest))]
    [JsonSerializable(typeof(SendRegistrationCodeRequest))]
    [JsonSerializable(typeof(CreateAccountRequest))]
    [JsonSerializable(typeof(ChangePasswordRequest))]
    [JsonSerializable(typeof(LoginDto))]
    [JsonSerializable(typeof(VerifyForgetCodeDto))]
    [JsonSerializable(typeof(VerifyRegistrationCodeDto))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
