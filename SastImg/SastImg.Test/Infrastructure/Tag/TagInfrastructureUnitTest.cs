using SastImg.Infrastructure.Domain.TagEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Test.Infrastructure.Tag
{
    [TestClass]
    public sealed class TagInfrastructureUnitTest
    {
        private const string ConnectionString =
            "Host=localhost;Port=5432;Database=sastimg;Username=postgres;Password=150524";

        [TestMethod]
        public async Task Query_AllTags_ReturnsNotEmpty()
        {
            using IDbConnectionFactory factory = new DbConnectionFactory(ConnectionString);
            TagQueryRepository _repository = new(factory);
            var tagDtos = await _repository.GetAllTagsAsync();

            int count = tagDtos.Count();

            Assert.AreNotEqual(0, count);
        }

        [TestMethod]
        [DataRow([new long[1] { -1, }])]
        [DataRow([new long[5] { -1, -2, -3, -4, -5 }])]
        public async Task Query_MinusIdTags_ReturnsEmpty(long[] ids)
        {
            using IDbConnectionFactory factory = new DbConnectionFactory(ConnectionString);
            TagQueryRepository _repository = new(factory);
            var tagDtos = await _repository.GetTagsAsync(ids);

            int count = tagDtos.Count();

            Assert.AreEqual(0, count);
        }
    }
}
