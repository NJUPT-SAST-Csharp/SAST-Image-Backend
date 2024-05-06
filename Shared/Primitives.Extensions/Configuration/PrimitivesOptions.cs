using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;

namespace Primitives.Extensions.Configuration
{
    public sealed class PrimitivesOptions
    {
        private readonly IServiceCollection _services;
        private readonly HashSet<Assembly> _resolveAssemblies = [];

        internal Assembly[] Assemblies => [.. _resolveAssemblies];

        internal PrimitivesOptions(IServiceCollection services)
        {
            _services = services;
            AddDefaultBuses();
        }

        public bool AutoCommitAfterCommandHandled { get; set; } = false;

        public PrimitivesOptions ApplyCommandSender<TSender>()
            where TSender : class, ICommandRequestSender
        {
            _services.TryAddScoped<ICommandRequestSender, TSender>();
            return this;
        }

        public PrimitivesOptions ApplyQuerySender<TSender>()
            where TSender : class, IQueryRequestSender
        {
            _services.TryAddScoped<IQueryRequestSender, TSender>();
            return this;
        }

        public PrimitivesOptions ApplyDomainEventPublisher<TPublisher>()
            where TPublisher : class, IDomainEventPublisher
        {
            _services.TryAddScoped<IDomainEventPublisher, TPublisher>();
            return this;
        }

        public PrimitivesOptions AddResolversFromAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                _resolveAssemblies.Add(assembly);
            }

            return this;
        }

        public PrimitivesOptions AddResolverFromAssembly(Assembly assembly)
        {
            _resolveAssemblies.Add(assembly);
            return this;
        }

        public PrimitivesOptions AddResolversFromAssemblyContaining<T>()
        {
            _resolveAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        public PrimitivesOptions AddUnitOfWork<TUnitOfWork>()
            where TUnitOfWork : class, IUnitOfWork
        {
            _services.TryAddScoped<IUnitOfWork, TUnitOfWork>();
            return this;
        }

        public PrimitivesOptions AddUnitOfWorkWithDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            _services.TryAddScoped<IUnitOfWork, DefaultUnitOfWork<TDbContext>>();

            return this;
        }

        public PrimitivesOptions AddUnitOfWorkWithDbContext<TWriteDbContext, TQueryDbContext>()
            where TWriteDbContext : DbContext
            where TQueryDbContext : DbContext
        {
            _services.TryAddScoped<
                IUnitOfWork,
                DefaultUnitOfWork<TWriteDbContext, TQueryDbContext>
            >();

            return this;
        }

        internal PrimitivesOptions AddDefaultBuses()
        {
            ApplyCommandSender<DefaultEventBus>();
            ApplyQuerySender<DefaultEventBus>();
            ApplyDomainEventPublisher<DefaultEventBus>();

            return this;
        }
    }
}
