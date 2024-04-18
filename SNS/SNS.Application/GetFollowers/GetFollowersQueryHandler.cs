using Shared.Primitives.Query;

namespace SNS.Application.GetFollowers
{
    internal class GetFollowersQueryHandler(IFollowerRepository repository)
        : IQueryRequestHandler<GetFollowersQuery, IEnumerable<FollowerDto>>
    {
        private readonly IFollowerRepository _repository = repository;

        public Task<IEnumerable<FollowerDto>> Handle(
            GetFollowersQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetFollowersAsync(request.UserId, cancellationToken);
        }
    }
}
