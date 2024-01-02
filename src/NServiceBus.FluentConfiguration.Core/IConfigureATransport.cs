using System;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IConfigureATransport<T> where T : TransportDefinition
    {

        IConfigureAnEndpoint WithRouting<TDefault>() where TDefault : IDefaultRoutingConfiguration<T>, new();
        IConfigureAnEndpoint WithRouting(Action<RoutingSettings<T>> routingConfigurationAction);
        IConfigureAnEndpoint WithRouting<TDefault>(Action<RoutingSettings<T>> routingConfigurationAction) where TDefault : IDefaultRoutingConfiguration<T>, new();

    }
   
}
