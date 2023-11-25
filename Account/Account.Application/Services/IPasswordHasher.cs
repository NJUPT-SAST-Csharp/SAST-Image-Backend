namespace Account.Application.Services
{
    public interface IPasswordHasher
    {
        public byte[] Hash(string password);
        public Task<byte[]> HashAsync(string password);
        public bool Validate(string password, byte[] passwordHash);
        public Task<bool> ValidateAsync(string password, byte[] passwordHash);
    }
}
