using Microsoft.AspNetCore.Http;
using Shared.Primitives.Query;
using Shared.Response.Builders;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    internal class GetUserBriefInfoQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserBriefInfoQuery, IResult>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<IResult> Handle(
            GetUserBriefInfoQuery request,
            CancellationToken cancellationToken
        )
        {
            var dto = await _repository.GetUserBriefInfoAsync(request.Username, cancellationToken);

            return Responses.DataOrNotFound(dto);
        }
    }
}
