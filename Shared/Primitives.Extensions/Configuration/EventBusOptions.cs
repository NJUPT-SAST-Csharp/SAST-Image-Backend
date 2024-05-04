using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;

namespace Primitives.Configuration
{
    public sealed class EventBusOptions
    {
        internal List<ServiceDescriptor> Services { get; private set; } = [];
        internal Assembly[] Assemblies => [.. _resolveAssemblies];

        private readonly HashSet<Assembly> _resolveAssemblies = [];

        public EventBusOptions AddCommandSender<TSender>()
            where TSender : class, ICommandRequestSender
        {
            if (Services.Any(s => s.ServiceType == typeof(ICommandRequestSender)))
            {
                return this;
            }

            Services.Add(ServiceDescriptor.Singleton<ICommandRequestSender, TSender>());
            return this;
        }

        public EventBusOptions AddQuerySender<TSender>()
            where TSender : class, IQueryRequestSender
        {
            if (Services.Any(s => s.ServiceType == typeof(ICommandRequestSender)))
            {
                return this;
            }

            Services.Add(ServiceDescriptor.Singleton<IQueryRequestSender, TSender>());
            return this;
        }

        public EventBusOptions AddDomainEventPublisher<TPublisher>()
            where TPublisher : class, IDomainEventPublisher
        {
            if (Services.Any(s => s.ServiceType == typeof(IDomainEventPublisher)))
            {
                return this;
            }

            Services.Add(ServiceDescriptor.Singleton<IDomainEventPublisher, TPublisher>());
            return this;
        }

        public EventBusOptions AddResolversFromAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                _resolveAssemblies.Add(assembly);
            }

            return this;
        }

        public EventBusOptions AddResolverFromAssemblyContaining<T>()
        {
            _resolveAssemblies.Add(typeof(T).Assembly);
            return this;
        }

        internal EventBusOptions AddDefaultBuses()
        {
            AddCommandSender<DefaultEventBus>();
            AddQuerySender<DefaultEventBus>();
            AddDomainEventPublisher<DefaultEventBus>();

            return this;
        }
    }
}
