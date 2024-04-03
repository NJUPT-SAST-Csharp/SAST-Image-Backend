using Square.Application.TopicServices;
using Square.Domain;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.ColumnAggregate.Events;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices.Models
{
    public sealed class ColumnModel
    {
        public ColumnId Id { get; private init; }
        public ColumnText Text { get; private set; }
        public UserId AuthorId { get; private init; }
        public TopicId TopicId { get; private init; }
        public DateTime PublishedAt { get; private set; }
        public ICollection<ColumnImage> Images { get; private set; }
        public HashSet<ColumnLike> Likes { get; private init; }

        public static async Task<ColumnModel> CreateNewColumnAsync(
            NewColumnAddedEvent e,
            IColumnImageStorage storage
        )
        {
            var images = await storage.UploadImagesAsync(e.Images);

            return new ColumnModel
            {
                Id = e.Id,
                Text = e.ColumnText,
                AuthorId = e.AuthorId,
                TopicId = e.TopicId,
                PublishedAt = DateTime.UtcNow,
                Images = images.ToList(),
                Likes = []
            };
        }

        public async Task UpdateAsync(ExistingColumnUpdatedEvent e, IColumnImageStorage storage)
        {
            var deleteTask = storage.DeleteImagesAsync(Images);

            var images = await storage.UploadImagesAsync(e.Images);

            Text = e.ColumnText;
            Images = images.ToList();
            PublishedAt = DateTime.UtcNow;

            await deleteTask;
        }

        public void Like(ColumnLikedEvent e)
        {
            Likes.Add(new ColumnLike(e.ColumnId, e.LikedBy));
        }

        public void Unlike(ColumnUnlikedEvent e)
        {
            Likes.RemoveWhere(like => like.UserId == e.UserId);
        }
    }
}
