using System.Text;
using Account.Application.Services;
using Konscious.Security.Cryptography;

namespace Account.Infrastructure
{
    internal sealed class PasswordHasher : IPasswordHasher
    {
        public byte[] Hash(string password)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4192;
            return argon.GetBytes(32);
        }

        public Task<byte[]> HashAsync(string password)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4192;
            return argon.GetBytesAsync(32);
        }

        public bool Validate(string password, byte[] passwordHash)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4192;
            var bytes = argon.GetBytes(32);
            return bytes.SequenceEqual(passwordHash);
        }

        public async Task<bool> ValidateAsync(string password, byte[] passwordHash)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4192;
            var bytes = await argon.GetBytesAsync(32);
            return bytes.SequenceEqual(passwordHash);
        }
    }
}
