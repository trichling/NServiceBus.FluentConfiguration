using System;
using NServiceBus.Transport;

namespace NServiceBus.Configuration.WebApi
{

    public interface IConfigureATransport<T> where T : TransportDefinition, new()
    {

        TransportExtensions<T> Transport { get;  }

        IConfigureAnEndpoint WithRouting(Action<RoutingSettings<T>> routingConfigurationAction);
        IConfigureAnEndpoint WithRouting<TDefault>(Action<RoutingSettings<T>> routingConfigurationAction) where TDefault : IDefaultRoutingConfiguration<T>, new();

    }
   
}
