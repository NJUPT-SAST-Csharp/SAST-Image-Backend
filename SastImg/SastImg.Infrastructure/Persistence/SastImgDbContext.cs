using Microsoft.EntityFrameworkCore;
using SastImg.Domain.Entities;

namespace SastImg.Infrastructure.Persistence
{
    public class SastImgDbContext : DbContext
    {
        public SastImgDbContext(DbContextOptions<SastImgDbContext> options)
            : base(options) { }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
