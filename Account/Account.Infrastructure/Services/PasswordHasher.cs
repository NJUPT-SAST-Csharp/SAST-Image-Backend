using System.Text;
using Account.Application.Services;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Account.Infrastructure.Services
{
    internal sealed class PasswordHasher(IConfiguration configuration) : IPasswordHasher
    {
        private readonly int _iterations = configuration
            .GetSection("DEA")
            .GetValue<int>("Iterations");
        private readonly int _memorySize = configuration
            .GetSection("DEA")
            .GetValue<int>("MemorySize");
        private readonly string _salt =
            configuration.GetSection("DEA").GetValue<string>("Salt")
            ?? throw new ArgumentNullException(nameof(_salt));

        public byte[] Hash(string password)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.Salt = Encoding.Default.GetBytes(_salt);
            return argon.GetBytes(32);
        }

        public Task<byte[]> HashAsync(string password)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.Salt = Encoding.Default.GetBytes(_salt);
            return argon.GetBytesAsync(32);
        }

        public bool Validate(string password, byte[] passwordHash)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.Salt = Encoding.Default.GetBytes(_salt);
            var bytes = argon.GetBytes(32);
            return bytes.SequenceEqual(passwordHash);
        }

        public async Task<bool> ValidateAsync(string password, byte[] passwordHash)
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password));
            argon.Iterations = 16;
            argon.MemorySize = 4096;
            argon.Salt = Encoding.Default.GetBytes(_salt);
            var bytes = await argon.GetBytesAsync(32);
            return bytes.SequenceEqual(passwordHash);
        }
    }
}
