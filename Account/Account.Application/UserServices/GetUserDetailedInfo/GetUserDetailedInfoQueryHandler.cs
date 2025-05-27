using Mediator;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.UserServices.GetUserDetailedInfo;

public sealed class GetUserDetailedInfoQueryHandler(IUserQueryRepository repository)
    : IQueryHandler<GetUserDetailedInfoQuery, IResult>
{
    public async ValueTask<IResult> Handle(
        GetUserDetailedInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        var dto = await repository.GetUserDetailedInfoAsync(request.Username, cancellationToken);

        return Responses.DataOrNotFound(dto);
    }
}
