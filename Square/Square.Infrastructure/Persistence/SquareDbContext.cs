using Microsoft.EntityFrameworkCore;
using Square.Application.CategoryServices;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;
using Square.Infrastructure.Persistence.EntityTypeConfigurations;

namespace Square.Infrastructure.Persistence
{
    public sealed class SquareDbContext(DbContextOptions<SquareDbContext> options)
        : DbContext(options)
    {
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Column> Columns { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ColumnModel> ColumnModels { get; set; }

        public DbSet<TopicModel> TopicModels { get; set; }

        public DbSet<CategoryModel> CategoryModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TopicEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ColumnEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ColumnModelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TopicModelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryModelTypeConfiguration());
        }
    }
}
