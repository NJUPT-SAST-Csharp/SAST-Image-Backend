using Account.Application.UserServices.GetUserDetailedInfo;
using Mediator;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.UserServices.GetUserBriefInfo;

public sealed class GetUserBriefInfoQueryHandler(IUserQueryRepository repository)
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
                return Responses.DataOrNotFound(dto);
            }
            else
            {
                dto = await repository.GetUserDetailedInfoAsync(
                    request.Username!,
                    cancellationToken
                );
                return Responses.DataOrNotFound(dto);
            }
        }

        UserBriefInfoDto? briefInfo = null;

        if (request.Id.HasValue)
        {
            briefInfo = await repository.GetUserBriefInfoAsync(request.Id.Value, cancellationToken);
            return Responses.DataOrNotFound(briefInfo);
        }
        else
        {
            briefInfo = await repository.GetUserBriefInfoAsync(
                request.Username!,
                cancellationToken
            );
            return Responses.DataOrNotFound(briefInfo);
        }
    }
}
