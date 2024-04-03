using Square.Application.ColumnServices.Models;

namespace Square.Application.ColumnServices.Queries.GetColumn
{
    public sealed class ColumnDetailedDto
    {
        public long AuthorId { get; init; }

        public string? Text { get; init; }

        public IEnumerable<ColumnImage> Images { get; init; }

        public DateTime PublishedAt { get; init; }

        public static ColumnDetailedDto MapFrom(ColumnModel column)
        {
            return new ColumnDetailedDto
            {
                AuthorId = column.AuthorId.Value,
                Text = column.Text.Value,
                Images = column.Images,
                PublishedAt = column.PublishedAt
            };
        }
    }
}
