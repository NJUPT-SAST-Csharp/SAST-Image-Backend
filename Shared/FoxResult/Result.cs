using System.Diagnostics.CodeAnalysis;

namespace FoxResult
{
    public class Result
    {
        public Error Error { get; private init; } = Error.None;
        public bool IsSuccess { get; private init; } = true;
        public bool IsFailure => !IsSuccess;

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
    }

    public sealed class Result<T> : Result
    {
        internal Result(T? value)
        {
            Value = value;
        }

        [MemberNotNullWhen(false, nameof(Value))]
        public new bool IsFailure => !IsSuccess;

        public T? Value { get; }

        public bool TryGetValue([NotNullWhen(true)] out T? value)
        {
            if (IsSuccess)
            {
                value = Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
