using System.Diagnostics.CodeAnalysis;

namespace FoxResult
{
    public record class Result
    {
        public Error? Error { get; private init; } = Error.None;

        [MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess { get; private init; } = true;

        [MemberNotNullWhen(true, nameof(Error))]
        public virtual bool IsFailure => !IsSuccess;

        public static readonly Result Success = new();

        public static Result Fail(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Error cannot be Error.None", nameof(error));
            }

            return new() { IsSuccess = false, Error = error };
        }

        public static Result Fail(string message) =>
            new() { IsSuccess = false, Error = new(message) };

        public static Result<T> Fail<T>(Error<T> error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Error cannot be Error.None", nameof(error));
            }

            return new(default) { IsSuccess = false, Error = error };
        }

        public static Result<T> Return<T>(T value) => new(value);

        public static Result<T> From<T>(Result result)
        {
            if (result.IsSuccess)
            {
                throw new ArgumentException(
                    "Parameter 'result' must be failure when using 'Result.From'.",
                    nameof(result)
                );
            }

            return new Result<T>(default) { IsSuccess = false, Error = result.Error };
        }
    }

    public sealed record class Result<T> : Result
    {
        internal Result(T? value)
        {
            Value = value;
        }

        [MemberNotNullWhen(false, nameof(Value))]
        public override bool IsFailure => !IsSuccess;

        public T? Value { get; }

        public bool TryGetValue([NotNullWhen(true)] out T? value)
        {
            if (IsFailure)
            {
                value = default;
                return false;
            }
            else
            {
                value = Value;
                return true;
            }
        }
    }
}
