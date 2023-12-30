using SastImg.Application.ImageServices.SearchImages;

namespace SastImg.Infrastructure.QueryRepositories
{
    public readonly ref struct OrderOptions(long[] ids, SearchOrder order)
    {
        public OrderOptions()
            : this([], SearchOrder.ByDate) { }

        public readonly long[] Ids { get; } = ids;
        public readonly SearchOrder Order { get; } = order;
    }
}
