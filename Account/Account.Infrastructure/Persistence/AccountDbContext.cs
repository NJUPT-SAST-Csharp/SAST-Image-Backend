using Account.Domain.UserEntity;
using Account.Infrastructure.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Persistence
{
    public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
