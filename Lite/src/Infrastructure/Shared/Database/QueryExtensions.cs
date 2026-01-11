using Application.AlbumServices;
using Application.ImageServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;

namespace Infrastructure.Shared.Database;

internal static class QueryExtensions
{
    public static IQueryable<ImageModel> WhereIsAccessible(
        this IQueryable<ImageModel> images,
        Actor actor
    )
    {
        return images.Where(i =>
            i.AccessLevel >= AccessLevelValue.PublicReadOnly
            || i.AccessLevel >= AccessLevelValue.AuthReadOnly && actor.IsAuthenticated
            || i.AccessLevel == AccessLevelValue.Private
                && (
                    i.AuthorId == actor.Id.Value
                    || i.Collaborators.Contains(actor.Id.Value)
                    || actor.IsAdmin
                )
        );
    }

    public static IQueryable<AlbumModel> WhereIsAccessible(
        this IQueryable<AlbumModel> albums,
        Actor actor
    )
    {
        return albums.Where(a =>
            a.AccessLevel >= AccessLevelValue.PublicReadOnly
            || a.AccessLevel >= AccessLevelValue.AuthReadOnly && actor.IsAuthenticated
            || a.AccessLevel == AccessLevelValue.Private
                && (
                    a.AuthorId == actor.Id.Value
                    || a.Collaborators.Contains(actor.Id.Value)
                    || actor.IsAdmin
                )
        );
    }
}
