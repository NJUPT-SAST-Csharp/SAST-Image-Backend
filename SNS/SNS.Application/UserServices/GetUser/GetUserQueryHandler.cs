using Shared.Primitives.Query;

namespace SNS.Application.UserServices.GetUser
{
    internal sealed class GetUserQueryHandler(IUserQueryRepository repository)
        : IQueryRequestHandler<GetUserQuery, UserDto?>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<UserDto?> Handle(
            GetUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var user = await _repository.GetUserAsync(request.UserId, cancellationToken);
            return user;
        }
    }
}
