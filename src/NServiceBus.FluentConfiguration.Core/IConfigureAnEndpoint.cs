using System;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IConfigureAnEndpoint
    {

        EndpointConfiguration Configuration { get;  }

        IConfigureATransport<T> WithTransport<T>() where T : TransportDefinition, new();
        IConfigureATransport<T> WithTransport<T, TDefault>() where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new();
        IConfigureATransport<T> WithTransport<T>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new();
        IConfigureATransport<T> WithTransport<T, TDefault>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new();


        IConfigureAnEndpoint WithPersistence<T>() where T : PersistenceDefinition;
        IConfigureAnEndpoint WithPersistence<T, TDefault>() where T : PersistenceDefinition where TDefault : IDefaultPersistenceConfiguration<T>, new();
        IConfigureAnEndpoint WithPersistence<T>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition;
        IConfigureAnEndpoint WithPersistence<T, TDefault>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition where TDefault : IDefaultPersistenceConfiguration<T>, new();

        IConfigureAnEndpoint WithConventions(Action<ConventionsBuilder> conventionConfigurationAction);
        IConfigureAnEndpoint WithConventions<TDefault>() where TDefault : IDefaultConventionsConfiguration, new();
        IConfigureAnEndpoint WithConventions<TDefault>(Action<ConventionsBuilder> conventionConfigurationAction) where TDefault : IDefaultConventionsConfiguration, new();

        IManageAnEndpoint ManageEndpoint();
    }
}
