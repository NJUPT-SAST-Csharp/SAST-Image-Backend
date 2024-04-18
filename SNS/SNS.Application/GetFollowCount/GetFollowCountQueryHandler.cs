using Shared.Primitives.Query;

namespace SNS.Application.GetFollowCount
{
    internal sealed class GetFollowCountQueryHandler(IFollowCountRepository repository)
        : IQueryRequestHandler<GetFollowCountQuery, FollowCountDto>
    {
        private readonly IFollowCountRepository _repository = repository;

        public Task<FollowCountDto> Handle(
            GetFollowCountQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetFollowCountAsync(request.UserId, cancellationToken);
        }
    }
}
