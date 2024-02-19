using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserBriefInfo
{
    internal class GetUserBriefInfoQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserBriefInfoQuery, UserBriefInfoDto>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<UserBriefInfoDto> Handle(
            GetUserBriefInfoQuery request,
            CancellationToken cancellationToken
        )
        {
            var dto = await _repository.GetUserBriefInfoAsync(request.Username, cancellationToken);

            return dto;
        }
    }
}
