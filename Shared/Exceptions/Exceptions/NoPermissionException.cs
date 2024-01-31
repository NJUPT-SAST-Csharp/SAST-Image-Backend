namespace Exceptions.Exceptions
{
    public sealed class NoPermissionException(string? message = null) : Exception
    {
        public override string Message { get; } =
            message ?? "You don't have permission to complete the request.";
    }
}
