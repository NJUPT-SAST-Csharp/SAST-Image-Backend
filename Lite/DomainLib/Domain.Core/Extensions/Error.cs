using System.Net;

namespace Domain.Extensions;

public record class Error(string Message, int Code = 400)
{
    public IErrorExtensions Extensions { get; } = default!;

    public static readonly Error? None = null;

    public static readonly Error Forbidden =
        new(
            "You don't have enough permission to complete the action.",
            (int)HttpStatusCode.Forbidden
        );

    public static Error NotFound(string? message = null) =>
        new(message ?? "Couldn't find entity.", (int)HttpStatusCode.NotFound);

    public static Error NotFound<TEntity>() =>
        new($"Couldn't find entity [{typeof(TEntity).Name}].", (int)HttpStatusCode.NotFound);

    public static Error Conflict(string? message = null) =>
        new(message ?? "Entity properties conflict", (int)HttpStatusCode.Conflict);

    public static Error Conflict<TEntity>() =>
        new($"Entity [{typeof(TEntity).Name}] properties conflict.", (int)HttpStatusCode.Conflict);
}

public interface IErrorExtensions { }
