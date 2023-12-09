using Account.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.Infrastructure.Configurations
{
    internal sealed class AccountDbContextFactory : IDesignTimeDbContextFactory<AccountDbContext>
    {
        public AccountDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AccountDbContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql(args.First()).UseSnakeCaseNamingConvention();
            return new(optionsBuilder.Options);
        }
    }
}
