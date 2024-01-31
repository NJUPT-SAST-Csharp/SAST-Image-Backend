using System.Data;
using Dapper;
using SastImg.Application.TagServices;
using SastImg.Domain.TagEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal class TagQueryRepository(IDbConnectionFactory factory) : ITagQueryRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        public Task<IEnumerable<TagDto>> GetAllTagsAsync(
            CancellationToken cancellationToken = default
        )
        {
            const string sql = "SELECT " + "id AS Id, " + "name AS Name " + "FROM tags";

            return _connection.QueryAsync<TagDto>(sql);
        }

        public Task<IEnumerable<TagDto>> GetTagsAsync(
            TagId[] ids,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id AS Id, "
                + "name AS Name "
                + "FROM tags "
                + "WHERE id = ANY ( @array ) ";
            return _connection.QueryAsync<TagDto>(sql, new { array = ids.Select(id => id.Value) });
        }

        public Task<IEnumerable<TagDto>> SearchTagsAsync(
            string name,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id AS Id, "
                + "name AS Name "
                + "FROM tags "
                + "WHERE name ILIKE @name";
            return _connection.QueryAsync<TagDto>(sql, new { name = $"%{name}%" });
        }
    }
}
