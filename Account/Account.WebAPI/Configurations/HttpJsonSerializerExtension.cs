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
using Account.Application.Endpoints.UserEndpoints.ChangeProfile;
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
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            return services;
        }
    }

    [JsonSerializable(typeof(ClaimsPrincipal))]
    [JsonSerializable(typeof(BadRequestResponse))]
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(AuthorizeRequest))]
    [JsonSerializable(typeof(VerifyRegistrationCodeRequest))]
    [JsonSerializable(typeof(ResetPasswordRequest))]
    [JsonSerializable(typeof(VerifyForgetCodeRequest))]
    [JsonSerializable(typeof(SendForgetCodeRequest))]
    [JsonSerializable(typeof(SendRegistrationCodeRequest))]
    [JsonSerializable(typeof(ChangeProfileRequest))]
    [JsonSerializable(typeof(CreateAccountRequest))]
    [JsonSerializable(typeof(ChangePasswordRequest))]
    [JsonSerializable(typeof(DataResponse<LoginDto>))]
    [JsonSerializable(typeof(DataResponse<VerifyForgetCodeDto>))]
    [JsonSerializable(typeof(DataResponse<VerifyRegistrationCodeDto>))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
