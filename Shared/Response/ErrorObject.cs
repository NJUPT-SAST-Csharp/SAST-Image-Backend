namespace Shared.Response
{
    public sealed class ErrorObject
    {
        internal ErrorObject(string field, string message)
        {
            Field = field;
            Message = message;
        }

        public string Field { get; init; }
        public string Message { get; init; }
    }
}
