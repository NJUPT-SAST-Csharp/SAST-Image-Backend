using Account.Application.UserServices.GetUserDetailedInfo;
using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;
using Shared.Response.Builders;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    internal class GetUserBriefInfoQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserInfoQuery, IResult>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<IResult> Handle(
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
                    dto = await _repository.GetUserDetailedInfoAsync(
                        request.Id.Value,
                        cancellationToken
                    );
                    return Responses.DataOrNotFound(dto);
                }
                else
                {
                    dto = await _repository.GetUserDetailedInfoAsync(
                        request.Username!,
                        cancellationToken
                    );
                    return Responses.DataOrNotFound(dto);
                }
            }

            UserBriefInfoDto? briefInfo = null;

            if (request.Id.HasValue)
            {
                briefInfo = await _repository.GetUserBriefInfoAsync(
                    request.Id.Value,
                    cancellationToken
                );
                return Responses.DataOrNotFound(briefInfo);
            }
            else
            {
                briefInfo = await _repository.GetUserBriefInfoAsync(
                    request.Username!,
                    cancellationToken
                );
                return Responses.DataOrNotFound(briefInfo);
            }
        }
    }
}
