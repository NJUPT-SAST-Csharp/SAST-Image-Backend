using Account.Application.Services;
using Identity;
using Mediator;
using Microsoft.AspNetCore.Http;
using Response;

namespace Account.Application.Queries;

public sealed class GetUserInfoQuery(string? username, long? id, bool isDetailed) : IQuery<IResult>
{
    public string? Username { get; } = username;
    public UserId? Id { get; } = id.HasValue ? new(id.Value) : null;
    public bool IsDetailed { get; } = isDetailed;
}

internal sealed class GetUserBriefInfoQueryHandler(IUserQueryRepository repository)
    : IQueryHandler<GetUserInfoQuery, IResult>
{
    public async ValueTask<IResult> Handle(
        GetUserInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Username is null && request.Id is null)
        {
            return Results.NotFound();
        }

        if (request.IsDetailed)
        {
            UserDetailedInfoDto? dto = null;

            if (request.Id.HasValue)
            {
                dto = await repository.GetUserDetailedInfoAsync(
                    request.Id.Value,
                    cancellationToken
                );
                return Results.Extensions.Data(dto);
            }
            else
            {
                dto = await repository.GetUserDetailedInfoAsync(
                    request.Username!,
                    cancellationToken
                );
                return Results.Extensions.Data(dto);
            }
        }

        UserBriefInfoDto? briefInfo = null;

        if (request.Id.HasValue)
        {
            briefInfo = await repository.GetUserBriefInfoAsync(request.Id.Value, cancellationToken);
            return Results.Extensions.Data(briefInfo);
        }
        else
        {
            briefInfo = await repository.GetUserBriefInfoAsync(
                request.Username!,
                cancellationToken
            );
            return Results.Extensions.Data(briefInfo);
        }
    }
}

public sealed class UserBriefInfoDto
{
    public required string Username { get; init; }
    public required string Nickname { get; init; }
}
