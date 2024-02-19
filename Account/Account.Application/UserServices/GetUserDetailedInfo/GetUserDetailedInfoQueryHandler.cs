using Shared.Primitives.Query;

namespace Account.Application.UserServices.GetUserDetailedInfo
{
    internal class GetUserDetailedInfoQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserDetailedInfoQuery, UserDetailedInfoDto>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<UserDetailedInfoDto> Handle(
            GetUserDetailedInfoQuery request,
            CancellationToken cancellationToken
        )
        {
            var dto = await _repository.GetUserDetailedInfoAsync(
                request.Username,
                cancellationToken
            );

            return dto;
        }
    }
}
