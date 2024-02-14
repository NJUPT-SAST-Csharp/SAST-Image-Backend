namespace SNS.WebAPI.Requests
{
    public readonly struct UpdateProfileRequest
    {
        public readonly string Nickname { get; init; }
        public readonly string Biography { get; init; }
    }
}
