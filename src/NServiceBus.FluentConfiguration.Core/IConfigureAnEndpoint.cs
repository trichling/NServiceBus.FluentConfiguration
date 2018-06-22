using System;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IConfigureAnEndpoint
    {

        EndpointConfiguration Configuration { get;  }

        IConfigureATransport<T> WithTransport<T>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new();

        IConfigureATransport<T> WithTransport<T, TDefault>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new();

        IConfigureAnEndpoint WithPersistence<T>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition;
        IConfigureAnEndpoint WithPersistence<T, TDefault>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition where TDefault : IDefaultPersistenceConfiguration<T>, new();

        IManageAnEndpoint ManageEndpoint();

        
    }
}
