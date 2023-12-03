namespace Account.Entity.User.Repositories
{
    public interface IUserCheckRepository
    {
        public Task<bool> CheckEmailExistenceAsync(string email);
        public Task<bool> CheckUsernameExistenceAsync(string email);
        public Task<bool> CheckSignInAsync(string username, byte[] password);
    }
}
