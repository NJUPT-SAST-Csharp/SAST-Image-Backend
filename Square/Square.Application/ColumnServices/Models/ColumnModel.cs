using Square.Domain;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices.Models
{
    public sealed class ColumnModel
    {
        public ColumnId Id { get; init; }
        public ColumnText Text { get; init; }
        public UserId AuthorId { get; init; }
        public TopicId TopicId { get; init; }
        public DateTime PublishedAt { get; init; }
        public ICollection<ColumnImage> Images { get; init; }
    }
}
