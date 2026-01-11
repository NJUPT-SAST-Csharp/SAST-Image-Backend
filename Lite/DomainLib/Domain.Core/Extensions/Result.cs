using System.Diagnostics.CodeAnalysis;

namespace Domain.Extensions;

public record class Result(Error? Error = null)
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error == null;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public static readonly Result Success = new();

    public static Result Fail(string message) => new(new Error(message));

    public static Result Fail(Error error) => new(error);

    public static Result<T> Fail<T>(string message) => new(default) { Error = new(message) };

    public static Result<T> Fail<T>(Error error) => new(default) { Error = error };

    public static Result<T> Data<T>(T value) => new(value);
}

public sealed record class Result<T> : Result
{
    [MemberNotNullWhen(true, nameof(Value))]
    public new bool IsSuccess => Error == null;

    [MemberNotNullWhen(false, nameof(Value))]
    public new bool IsFailure => !IsSuccess;

    public static new Result<T> Fail(string message) => new(default(T)) { Error = new(message) };

    public static new Result<T> Fail(Error error) => new(default(T)) { Error = error };

    public T? Value { get; init; }

    public Result(T? value)
    {
        Value = value;
    }
}
