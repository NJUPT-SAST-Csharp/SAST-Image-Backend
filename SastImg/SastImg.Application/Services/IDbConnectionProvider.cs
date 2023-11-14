using System.Data;

namespace SastImg.Application.Services
{
    public interface IDbConnectionProvider
    {
        IDbConnection DbConnection { get; }
    }
}
