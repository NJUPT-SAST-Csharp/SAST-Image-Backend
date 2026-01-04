using System.Text;
using Storage.Infrastructure.Models;

namespace Storage.Test;

[TestClass]
public class FileTokenTest
{
    private const string bucketName = "test-bucket";
    private static FileToken validToken = null!;
    private static readonly byte[] secret = Encoding.ASCII.GetBytes(nameof(secret));

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        bool result = FileToken.TryCreateNew(bucketName, secret, out validToken!);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CreateNewToken_WithValidBucketName_ReturnTrue()
    {
        string validBucketName = bucketName;

        bool result = FileToken.TryCreateNew(validBucketName, secret, out var token);

        Assert.IsTrue(result);
        Assert.IsNotNull(token);
    }

    [TestMethod]
    public void CreateNewToken_WithInvalidBucketName_ReturnFalse()
    {
        string invalidBucketName = new('a', FileToken.MaxBucketNameLength + 1); // Exceeds max length

        bool result = FileToken.TryCreateNew(invalidBucketName, secret, out var token);

        Assert.IsFalse(result);
        Assert.IsNull(token);
    }

    [TestMethod]
    public void CreateNewToken_WithoutTime_ExpireAtMaxValue()
    {
        FileToken.TryCreateNew(bucketName, secret, out var token);

        Assert.IsNotNull(token);
        Assert.AreEqual(token.ExpireAt, DateTime.MaxValue);
    }

    [TestMethod]
    public void CreateNewToken_WithInvalidTime_ExpireAtMaxValue()
    {
        var invalidTime = TimeSpan.FromTicks(long.MaxValue);

        FileToken.TryCreateNew(bucketName, invalidTime, secret, out var token);

        Assert.IsNotNull(token);
        Assert.AreEqual(token.ExpireAt, DateTime.MaxValue);
    }

    [TestMethod]
    public void CreateNewToken_WithValidTime_ExpireAtSpecificValue()
    {
        var validTime = TimeSpan.FromDays(30);
        int expectedExpireDay = (DateTime.UtcNow + validTime).Day;

        FileToken.TryCreateNew(bucketName, validTime, secret, out var token);

        Assert.IsNotNull(token);
        Assert.AreEqual(expectedExpireDay, token.ExpireAt.Day);
    }

    [TestMethod]
    public void Validate_WithValidToken_ReturnTrue()
    {
        string validTokenString = validToken.Value;

        bool result = FileToken.TryValidate(validTokenString, secret, out var token);

        Assert.IsTrue(result);
        Assert.IsNotNull(token);
    }

    [TestMethod]
    public void Validate_WithInvalidToken_ReturnFalse()
    {
        string invalidTokenString = nameof(invalidTokenString);

        bool result = FileToken.TryValidate(invalidTokenString, secret, out var token);

        Assert.IsFalse(result);
        Assert.IsNull(token);
    }

    [TestMethod]
    public void Validate_WithInvalidSecret_ReturnFalse()
    {
        byte[] invalidSecret = Encoding.ASCII.GetBytes(nameof(invalidSecret));

        bool result = FileToken.TryValidate(validToken.Value, invalidSecret, out var token);

        Assert.IsFalse(result);
        Assert.IsNull(token);
    }
}
