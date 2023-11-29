using Account.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.Infrastructure.Configurations
{
    internal sealed class AccountDbContextFactory : IDesignTimeDbContextFactory<AccountDbContext>
    {
        public AccountDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountDbContext>();
            optionsBuilder
                .UseNpgsql(
                    "Host=localhost;Port=5432;Database=sastimg_account;Username=postgres;Password=150524"
                )
                .UseSnakeCaseNamingConvention();

            return new AccountDbContext(optionsBuilder.Options);
        }
    }
}
