using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.Infrastructure.Persistence.Migrations
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
