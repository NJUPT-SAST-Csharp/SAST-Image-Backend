using System.Text.Json.Serialization;

namespace Account.Application.Configurations
{
    [JsonSerializable(typeof(int[]))]
    [JsonSerializable(typeof(int))]
    public partial class AppJsonSerializerContext : JsonSerializerContext { }
}
