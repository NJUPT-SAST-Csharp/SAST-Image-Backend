using Microsoft.EntityFrameworkCore;
using Square.Application.CategoryServices;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Square.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Square.Infrastructure.Persistence
{
    internal sealed class SquareQueryDbContext(DbContextOptions<SquareQueryDbContext> options)
        : DbContext(options)
    {
        public DbSet<ColumnModel> Columns { get; set; }

        public DbSet<TopicModel> Topics { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("query");
            modelBuilder.ApplyConfiguration(new ColumnModelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TopicModelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryModelTypeConfiguration());
        }
    }
}
