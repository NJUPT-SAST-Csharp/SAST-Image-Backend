using System.Text.Json.Serialization;
using Account.Application.Users.Login;
using Account.WebAPI.Endpoints.Login;
using Microsoft.AspNetCore.Mvc;

namespace Account.Application.Configurations
{
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(ObjectResult))]
    [JsonSerializable(typeof(IActionResult))]
    [JsonSerializable(typeof(LoginDto))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
