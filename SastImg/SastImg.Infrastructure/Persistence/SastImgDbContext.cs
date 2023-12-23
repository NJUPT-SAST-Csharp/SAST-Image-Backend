using Microsoft.EntityFrameworkCore;
using SastImg.Domain;
using SastImg.Domain.Categories;

namespace SastImg.Infrastructure.Persistence
{
    public sealed class SastImgDbContext : DbContext
    {
        public SastImgDbContext(DbContextOptions<SastImgDbContext> options)
            : base(options) { }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
