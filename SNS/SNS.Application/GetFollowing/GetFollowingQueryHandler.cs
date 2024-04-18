using Shared.Primitives.Query;

namespace SNS.Application.GetFollowing
{
    internal sealed class GetFollowingQueryHandler(IFollowingRepository repository)
        : IQueryRequestHandler<GetFollowingQuery, IEnumerable<FollowingDto>>
    {
        private readonly IFollowingRepository _repository = repository;

        public Task<IEnumerable<FollowingDto>> Handle(
            GetFollowingQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetFollowingAsync(request.UserId, cancellationToken);
        }
    }
}
