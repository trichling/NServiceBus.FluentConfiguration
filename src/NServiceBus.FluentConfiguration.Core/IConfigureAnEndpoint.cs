using System;
using NServiceBus.FluentConfiguration.Core.Profiles;
using NServiceBus.Persistence;
using NServiceBus.Serialization;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IConfigureAnEndpoint
    {

        EndpointConfiguration Configuration { get; }

        IConfigureAnEndpoint WithConfiguration(IEndpointConfigurationProfile profile);
        IConfigureAnEndpoint WithConfiguration<TDefault>() where TDefault : IDefaultEndpointConfiguration, new();
        IConfigureAnEndpoint WithConfiguration(Action<EndpointConfiguration> endpointConfigurationAction);
        IConfigureAnEndpoint WithConfiguration<TDefault>(Action<EndpointConfiguration> endpointConfigurationAction) where TDefault : IDefaultEndpointConfiguration, new();

        IConfigureATransport<T> WithTransport<T>(T transport) where T : TransportDefinition;
        IConfigureATransport<T> WithTransport<T, TDefault>(T transport) where T : TransportDefinition where TDefault : IDefaultTransportConfiguration<T>, new();
        IConfigureATransport<T> WithTransport<T>(T transport, Action<T> transportConfigurationAction) where T : TransportDefinition;
        IConfigureATransport<T> WithTransport<T, TDefault>(T transport, Action<T> transportConfigurationAction) where T : TransportDefinition where TDefault : IDefaultTransportConfiguration<T>, new();


        IConfigureAnEndpoint WithPersistence<T>() where T : PersistenceDefinition;
        IConfigureAnEndpoint WithPersistence<T, TDefault>() where T : PersistenceDefinition where TDefault : IDefaultPersistenceConfiguration<T>, new();
        IConfigureAnEndpoint WithPersistence<T>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition;
        IConfigureAnEndpoint WithPersistence<T, TDefault>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition where TDefault : IDefaultPersistenceConfiguration<T>, new();

        IConfigureAnEndpoint WithConventions(Action<ConventionsBuilder> conventionConfigurationAction);
        IConfigureAnEndpoint WithConventions<TDefault>() where TDefault : IDefaultConventionsConfiguration, new();
        IConfigureAnEndpoint WithConventions<TDefault>(Action<ConventionsBuilder> conventionConfigurationAction) where TDefault : IDefaultConventionsConfiguration, new();

        IConfigureAnEndpoint WithSerialization<T>() where T : SerializationDefinition, new();
        IConfigureAnEndpoint WithSerialization<T>(Action<SerializationExtensions<T>> serializationConfigurationAction) where T : SerializationDefinition, new();
        IConfigureAnEndpoint WithSerialization<T, TDefault>() where T : SerializationDefinition, new() where TDefault : IDefaultSerializationConfiguration<T>, new();
        IConfigureAnEndpoint WithSerialization<T, TDefault>(Action<SerializationExtensions<T>> serializationConfigurationAction) where T : SerializationDefinition, new() where TDefault : IDefaultSerializationConfiguration<T>, new();

        IManageAnEndpoint ManageEndpoint();
    }
}
