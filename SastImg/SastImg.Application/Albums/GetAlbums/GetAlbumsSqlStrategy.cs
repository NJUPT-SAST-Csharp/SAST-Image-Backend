namespace SastImg.Application.Albums.GetAlbums
{
    internal abstract class GetAlbumsSqlStrategy
    {
        public static GetAlbumsSqlStrategy Anonymous(int page, long authorId = 0) =>
            new GetAlbumsAnonymousStrategy(page, authorId);

        public static GetAlbumsSqlStrategy Common(int page, long requester, long authorId = 0) =>
            new GetAlbumsCommonStrategy(page, authorId, requester);

        public static GetAlbumsSqlStrategy Admin(int page, long authorId = 0) =>
            new GetAlbumsAdminStrategy(page, authorId);

        public abstract string SqlString { get; }
        public abstract object Parameters { get; }

        protected const int numPerPage = 20;

        protected const string prefix =
            "SELECT "
            + "id as AlbumId, "
            + "title as Title, "
            + "cover_uri as CoverUri, "
            + "accessibility as Accessibility, "
            + "author_id as AuthorId "
            + "FROM albums "
            + "WHERE accessibility = 0 ";

        protected const string suffix =
            " ORDER BY updated_at DESC " + "LIMIT @take " + "OFFSET @skip";

        private class GetAlbumsAnonymousStrategy(int page, long authorId = 0) : GetAlbumsSqlStrategy
        {
            string _sql =
                prefix + (authorId == 0 ? string.Empty : "AND author_id = @authorId ") + suffix;

            public override string SqlString => _sql;

            public override object Parameters { get; } =
                new
                {
                    take = numPerPage,
                    skip = numPerPage * page,
                    authorId
                };
        }

        private class GetAlbumsCommonStrategy(int page, long requesterId, long authorId = 0)
            : GetAlbumsSqlStrategy
        {
            string _sql =
                prefix
                + (
                    authorId == 0
                        ? "WHERE accessibility NOT 2 "
                        : "WHERE author_id = @authorId AND ( accessibility NOT 2 OR author_Id = @requesterId ) "
                )
                + suffix;

            public override string SqlString => _sql;

            public override object Parameters =>
                new
                {
                    take = numPerPage,
                    skip = numPerPage * page,
                    requesterId,
                    authorId
                };
        }

        private class GetAlbumsAdminStrategy(int page, long authorId = 0) : GetAlbumsSqlStrategy
        {
            string _sql =
                prefix + (authorId == 0 ? string.Empty : "author_id = @authorId ") + suffix;

            public override string SqlString => _sql;

            public override object Parameters =>
                new
                {
                    take = numPerPage,
                    skip = numPerPage * page,
                    authorId
                };
        }
    }
}
