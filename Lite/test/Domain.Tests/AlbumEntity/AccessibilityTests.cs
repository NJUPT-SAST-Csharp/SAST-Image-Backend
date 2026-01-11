using Domain.AlbumAggregate.AlbumEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AccessLevelTests
{
    [DataRow(AccessLevel.MaxValue + 1)]
    [DataRow(AccessLevel.MaxValue + 999)]
    [DataRow(AccessLevel.MinValue - 1)]
    [DataRow(AccessLevel.MinValue - 999)]
    [TestMethod]
    public void Return_False_When_Create_From_OutOfRange(int value)
    {
        var accessLevel = (AccessLevelValue)value;

        bool result = AccessLevel.TryCreateNew(accessLevel, out var _);

        result.ShouldBeFalse();
    }

    [DataRow(AccessLevel.MaxValue)]
    [DataRow(AccessLevel.MinValue)]
    [TestMethod]
    public void Should_Set_Value_When_Create_From_Valid(int value)
    {
        var accessLevelValue = (AccessLevelValue)value;

        bool result = AccessLevel.TryCreateNew(accessLevelValue, out var accessLevel);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void Should_Be_Equal_When_Same_Value()
    {
        int value = (AccessLevel.MinValue + AccessLevel.MinValue) / 2;
        AccessLevel.TryCreateNew((AccessLevelValue)value, out var accessLevel1);
        AccessLevel.TryCreateNew((AccessLevelValue)value, out var accessLevel2);

        accessLevel1.ShouldBe(accessLevel2);
    }

    [TestMethod]
    public void Should_Not_Be_Equal_When_Different_Value()
    {
        int value1 = AccessLevel.MinValue;
        int value2 = AccessLevel.MaxValue;
        AccessLevel.TryCreateNew((AccessLevelValue)value1, out var accessLevel1);
        AccessLevel.TryCreateNew((AccessLevelValue)value2, out var accessLevel2);

        accessLevel1.ShouldNotBe(accessLevel2);
    }
}
