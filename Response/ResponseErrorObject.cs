namespace Response
{
    public class ResponseErrorObject
    {
        internal ResponseErrorObject(string field, string message)
        {
            Field = field;
            Message = message;
        }

        public string Field { get; init; }
        public string Message { get; init; }
    }
}
