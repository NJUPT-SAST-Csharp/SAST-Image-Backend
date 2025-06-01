using System.Text.Json;
using System.Text.Json.Serialization;
using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.Application.UserServices.GetUserBriefInfo;
using Account.Application.UserServices.GetUserDetailedInfo;
using Account.WebAPI.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Response.ReponseObjects;
using Response.ResponseObjects;

namespace Account.WebAPI.Configurations;

public static class HttpJsonSerializerExtension
{
    public static IServiceCollection ConfigureJsonSerializer(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
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
[JsonSerializable(typeof(UpdateAvatarRequest))]
[JsonSerializable(typeof(SendForgetCodeRequest))]
[JsonSerializable(typeof(VerifyForgetCodeRequest))]
[JsonSerializable(typeof(GetUserInfoRequest))]
[JsonSerializable(typeof(NoContent))]
[JsonSerializable(typeof(ConflictResponse))]
[JsonSerializable(typeof(BadRequestResponse))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(VerifyRegistrationCodeDto))]
[JsonSerializable(typeof(CreateAccountDto))]
[JsonSerializable(typeof(UserBriefInfoDto))]
[JsonSerializable(typeof(UserDetailedInfoDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext { }
