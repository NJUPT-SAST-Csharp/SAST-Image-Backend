using System.Data;
using Dapper;
using SastImg.Application.CategoryServices;
using SastImg.Application.CategoryServices.GetAllCategory;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories;

internal sealed class CategoryQueryRepository(IDbConnectionFactory factory)
    : ICategoryQueryRepository
{
    private readonly IDbConnection _connection = factory.GetConnection();

    public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    )
    {
        const string sql =
            "SELECT "
            + "id AS Id, "
            + "name AS Name, "
            + "description AS Description "
            + "from categories";
        return _connection.QueryAsync<CategoryDto>(sql);
    }
}
