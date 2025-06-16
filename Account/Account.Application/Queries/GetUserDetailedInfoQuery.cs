using Account.Application.Services;
using Mediator;
using Microsoft.AspNetCore.Http;
using Response;

namespace Account.Application.Queries;

public sealed class GetUserDetailedInfoQuery(string username) : IQuery<IResult>
{
    public string Username { get; } = username;
}

internal sealed class GetUserDetailedInfoQueryHandler(IUserQueryRepository repository)
    : IQueryHandler<GetUserDetailedInfoQuery, IResult>
{
    public async ValueTask<IResult> Handle(
        GetUserDetailedInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        var dto = await repository.GetUserDetailedInfoAsync(request.Username, cancellationToken);

        return Results.Extensions.Data(dto);
    }
}

public sealed class UserDetailedInfoDto
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string Nickname { get; init; }
    public required string Biography { get; init; }
    public DateOnly? Birthday { get; init; }
    public Uri? Website { get; init; }
}
