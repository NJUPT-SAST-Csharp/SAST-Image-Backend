namespace Account.WebAPI.Requests;

public readonly struct UpdateHeaderRequest
{
    public readonly IFormFile HeaderFile { get; init; }
}
