using Square.Application.ColumnServices.Models;

namespace Square.Application.ColumnServices.Queries.GetColumns
{
    public sealed class ColumnDto
    {
        public long ColumnId { get; private init; }

        public long AuthorId { get; private init; }

        public DateTime PublishedAt { get; private init; }

        public static ColumnDto MapFrom(ColumnModel model)
        {
            return new ColumnDto
            {
                AuthorId = model.AuthorId.Value,
                PublishedAt = model.PublishedAt,
                ColumnId = model.Id.Value
            };
        }
    }
}
