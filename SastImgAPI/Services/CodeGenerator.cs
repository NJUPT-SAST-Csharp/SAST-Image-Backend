namespace SastImgAPI.Services
{
    public class CodeGenerator
    {
        public static string DefaultCode => GenerateNumericCode(6);

        public static string GenerateNumericCode(int length)
        {
            const string digits = "0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(digits, length).Select(s => s[random.Next(s.Length)]).ToArray()
            );
        }
    }
}
