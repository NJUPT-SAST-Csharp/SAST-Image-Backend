using Microsoft.EntityFrameworkCore;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Square.Infrastructure.Persistence
{
    internal sealed class SquareDbContext(DbContextOptions<SquareDbContext> options)
        : DbContext(options)
    {
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Column> Columns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("domain");
            modelBuilder.ApplyConfiguration(new TopicEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ColumnEntityTypeConfiguration());
        }
    }
}
