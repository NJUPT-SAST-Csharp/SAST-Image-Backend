using Account.Domain.UserEntity;
using Account.Domain.UserEntity.ValueObjects;
using Account.Infrastructure.Persistence.EntityTypeConfigurations;
using Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Primitives.Entity;
using Primitives.ValueObject;

namespace Account.Infrastructure.Persistence;

public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<UserId>().HaveConversion<OpenValueConverter<UserId>>();
        configurationBuilder
            .Properties<ImageToken>()
            .HaveConversion<OpenValueConverter<ImageToken, string>>();
        configurationBuilder
            .Properties<Username>()
            .HaveConversion<OpenValueConverter<Username, string>>();

        base.ConfigureConventions(configurationBuilder);
    }
}

internal sealed class OpenValueConverter<TObject, TValue>()
    : ValueConverter<TObject, TValue>(o => o.Value, v => new TObject { Value = v })
    where TObject : IValueObject<TObject, TValue>, new() { }

internal sealed class OpenValueConverter<TId>()
    : ValueConverter<TId, long>(o => o.Value, v => new TId { Value = v })
    where TId : ITypedId<TId>, new() { }
