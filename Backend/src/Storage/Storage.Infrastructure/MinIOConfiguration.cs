namespace Storage.Infrastructure;

public sealed class MinIOConfiguration
{
    public required string Endpoint { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
}
