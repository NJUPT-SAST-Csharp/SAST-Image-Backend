using System.Data;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Test.Infrastructure.Database;

[TestClass]
public sealed class QueryDbUnitTest
{
    const string ConnectionString =
        "Host=localhost;Port=5432;Database=sastimg;Username=postgres;Password=150524";

    [TestMethod]
    public void Create_Connection_ShouldOpen()
    {
        using DbConnectionFactory factory = new(ConnectionString);

        var connection = factory.GetConnection();

        Assert.AreEqual(ConnectionState.Open, connection.State);
    }

    [TestMethod]
    public void Dispose_ConnectionFactory_ShouldClosed()
    {
        DbConnectionFactory factory = new(ConnectionString);

        var connection = factory.GetConnection();
        factory.Dispose();

        Assert.AreEqual(ConnectionState.Closed, connection.State);
    }
}
