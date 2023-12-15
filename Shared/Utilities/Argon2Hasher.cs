using System.Text;
using Konscious.Security.Cryptography;

namespace Utilities
{
    public static class Argon2Hasher
    {
        public static byte[] Hash(string password, byte[] salt)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = salt;
            return argon.GetBytes(32);
        }

        public static Task<byte[]> HashAsync(string password, byte[] salt)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = salt;
            return argon.GetBytesAsync(32);
        }

        public static bool Validate(string password, byte[] passwordHash, byte[] salt)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = salt;
            var bytes = argon.GetBytes(32);
            return bytes.SequenceEqual(passwordHash);
        }

        public static async Task<bool> ValidateAsync(
            string password,
            byte[] passwordHash,
            byte[] salt
        )
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = salt;
            var bytes = await argon.GetBytesAsync(32);

            return bytes.SequenceEqual(passwordHash);
        }

        public static byte[] RandomSalt => RegenerateSalt(new byte[8]);

        public static byte[] RegenerateSalt(byte[] salt)
        {
            Random.Shared.NextBytes(salt);
            return salt;
        }
    }
}
