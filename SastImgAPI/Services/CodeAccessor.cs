using IdGen;
using Microsoft.AspNetCore.WebUtilities;

namespace SastImgAPI.Services
{
    public class CodeAccessor
    {
        private static readonly IdGenerator _idGenerator = new(233);
        public static string GenerateDefaultNumericCode => GenerateNumericCode(6);
        public static long GenerateSnowflakeId => _idGenerator.First();

        public static string GenerateNumericCode(int length)
        {
            const string digits = "0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(digits, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }

        public static string ToBase64String(long id)
        {
            var bytes = BitConverter.GetBytes(id);
            string base64 = WebEncoders.Base64UrlEncode(bytes);
            return base64;
        }

        public static long ToLongId(string base64)
        {
            byte[] bytes = WebEncoders.Base64UrlDecode(base64);
            long id = BitConverter.ToInt64(bytes);
            return id;
        }
    }
}
