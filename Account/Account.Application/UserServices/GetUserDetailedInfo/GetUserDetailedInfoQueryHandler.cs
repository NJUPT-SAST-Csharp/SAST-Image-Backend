using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;
using Shared.Response.Builders;

namespace Account.Application.UserServices.GetUserDetailedInfo
{
    internal class GetUserDetailedInfoQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserDetailedInfoQuery, IResult>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<IResult> Handle(
            GetUserDetailedInfoQuery request,
            CancellationToken cancellationToken
        )
        {
            var dto = await _repository.GetUserDetailedInfoAsync(
                request.Username,
                cancellationToken
            );

            return Responses.DataOrNotFound(dto);
        }
    }
}
