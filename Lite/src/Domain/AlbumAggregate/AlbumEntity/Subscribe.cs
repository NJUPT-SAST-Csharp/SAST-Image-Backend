using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.AlbumEntity;

public sealed record class Subscribe(AlbumId Album, UserId User) { }

internal static class SubscribesExtensions
{
    public static bool ContainsUser<TList>(this TList subscribes, UserId userId)
        where TList : IList<Subscribe>
    {
        return subscribes.Any(subscribe => subscribe.User == userId);
    }

    public static bool NotContainsUser<TList>(this TList subscribes, UserId userId)
        where TList : IList<Subscribe>
    {
        return subscribes.ContainsUser(userId) == false;
    }

    public static bool RemoveUser<TList>(this TList subscribes, UserId userId)
        where TList : IList<Subscribe>
    {
        for (int i = 0; i < subscribes.Count; i++)
        {
            if (subscribes[i].User == userId)
            {
                subscribes.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
