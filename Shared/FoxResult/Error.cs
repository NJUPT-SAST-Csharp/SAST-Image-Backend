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

        public static readonly Error? None = null;

        public static readonly Error Forbidden =
            new("You don't have enough permission to complete the action.", 403);

        public static Error NotFound(string? message = null) =>
            new(message ?? "Couldn't find entity.", 404);

        public static Error<TEntity> NotFound<TEntity>(TEntity? entity = default) =>
            new($"Couldn't find entity [{typeof(TEntity).Name}].", 404);

        public static Error Conflict(string? message = null) =>
            new(message ?? "Entity properties conflict", 409);

        public static Error<TEntity> Conflict<TEntity>(TEntity? entity = default) =>
            new($"Entity [{typeof(TEntity).Name}] properties conflict.", 409);
    }

    public sealed record class Error<T> : Error
    {
        internal Error(string message, int code = 400)
            : base(message, code) { }
    }
}
