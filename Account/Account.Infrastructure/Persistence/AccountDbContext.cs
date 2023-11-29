using Account.Entity.User;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Persistence
{
    public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options)
        : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(u => u.Profile);
            base.OnModelCreating(modelBuilder);
        }
    }
}
