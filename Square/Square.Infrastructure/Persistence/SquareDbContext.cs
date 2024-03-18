using Microsoft.EntityFrameworkCore;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Infrastructure.Persistence
{
    internal sealed class SquareDbContext(DbContextOptions<SquareDbContext> options)
        : DbContext(options)
    {
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SquareDbContext).Assembly);
        }
    }
}
