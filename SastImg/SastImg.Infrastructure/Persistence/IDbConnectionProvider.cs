using System.Data;

namespace SastImg.Infrastructure.Persistence
{
    public interface IDbConnectionProvider
    {
        IDbConnection DbConnection { get; }
    }
}
