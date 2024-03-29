namespace FoxResult
{
    public record class Error
    {
        public Error(string message, int code = 400)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; internal init; }
        public int Code { get; internal init; }

        public static readonly Error None = new(string.Empty);

        public static readonly Error Forbidden =
            new("You don't have enough permission to complete the action.", 403);

        public static Error NotFound(string? entityName = null) =>
            new($"Couldn't find entity {entityName}.", 404);

        public static Error<TEntity> NotFound<TEntity>(TEntity? entity = default) =>
            new($"Couldn't find entity {typeof(TEntity).Name}.", 404);
    }

    public sealed record class Error<T> : Error
    {
        internal Error(string message, int code = 400)
            : base(message, code) { }
    }
}
