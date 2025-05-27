using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Primitives.Behaviors;

namespace Primitives;

public sealed class PrimitivesOptions
{
    private readonly IServiceCollection services;

    internal PrimitivesOptions(IServiceCollection services)
    {
        this.services = services;
    }

    public PrimitivesOptions AddDefaultExceptionHandler()
    {
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<GeneralExceptionHandler>();

        return this;
    }

    public PrimitivesOptions AddUnitOfWork<TUnitOfWork>()
        where TUnitOfWork : class, IUnitOfWork
    {
        services.TryAddScoped<IUnitOfWork, TUnitOfWork>();
        services.TryAddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPostProcessor<,>));

        return this;
    }

    public PrimitivesOptions AddUnitOfWorkWithDbContext<TDbContext>()
        where TDbContext : DbContext
    {
        AddUnitOfWork<DefaultUnitOfWork<TDbContext>>();

        return this;
    }

    public PrimitivesOptions AddUnitOfWorkWithDbContext<TWriteDbContext, TQueryDbContext>()
        where TWriteDbContext : DbContext
        where TQueryDbContext : DbContext
    {
        AddUnitOfWork<DefaultUnitOfWork<TWriteDbContext, TQueryDbContext>>();

        return this;
    }
}
