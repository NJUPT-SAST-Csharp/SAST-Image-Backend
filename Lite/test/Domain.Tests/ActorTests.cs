using Domain.Shared;

namespace Domain.Tests;

internal class ActorTests { }

internal static class TestActor
{
    public const long AuthorId = 11111;
    public const long AdminId = 99999;
    public const long VisitorId = 55555;

    extension(Actor actor)
    {
        public static Actor New(long id, bool isAdmin = false) =>
            new()
            {
                Id = new(id),
                IsAdmin = isAdmin,
                IsAuthenticated = true,
            };

        public static Actor Author => Actor.New(AuthorId);
        public static Actor Visitor => Actor.New(VisitorId);
        public static Actor Admin => Actor.New(AdminId);
    }
}
