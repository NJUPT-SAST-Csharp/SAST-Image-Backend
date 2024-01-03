using System.Data;
using Dapper;
using SastImg.Application.TagServices;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.Domain.TagEntity
{
    internal class TagQueryRepository(IDbConnectionFactory factory) : ITagQueryRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        public Task<IEnumerable<TagDto>> GetAllTagsAsync(
            CancellationToken cancellationToken = default
        )
        {
            const string sql = "SELECT " + "id AS Id, " + "name AS Name " + "from tags";

            return _connection.QueryAsync<TagDto>(sql);
        }

        public Task<IEnumerable<TagDto>> GetTagsAsync(
            long[] ids,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "t.id AS Id, "
                + "t.name AS Name "
                + "FROM tags AS t "
                + "INNER JOIN unnest( @array ) "
                + "WITH ORDINALITY AS arr(id, ord) "
                + "ON t.id = arr.id "
                + "ORDER BY arr.ord;";
            return _connection.QueryAsync<TagDto>(sql, new { array = ids });
        }

        public Task<IEnumerable<TagDto>> SearchTagsAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id AS Id, "
                + "name AS Name, "
                + "from tags "
                + "WHERE name ILIKE @name";
            return _connection.QueryAsync<TagDto>(sql, new { name = $"%{name}%" });
        }
    }
}
