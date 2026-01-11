using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.ImageEntity;

public sealed record class Like(ImageId Image, UserId User) { }

internal static class LikeListExtensions
{
    public static bool ContainsUser(this List<Like> likes, UserId userId)
    {
        return likes.Any(like => like.User == userId);
    }

    public static bool NotContainsUser(this List<Like> likes, UserId userId)
    {
        return likes.ContainsUser(userId) == false;
    }

    public static void RemoveUser(this List<Like> likes, UserId userId)
    {
        likes.RemoveAll(like => like.User == userId);
    }
}
