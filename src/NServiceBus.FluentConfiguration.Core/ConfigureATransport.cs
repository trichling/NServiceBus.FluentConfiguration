using System;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{
    public class ConfigureATransport<T> : IConfigureATransport<T> where T : TransportDefinition
    {
        private readonly IConfigureAnEndpoint configureEndpoint;

        private RoutingSettings<T> Routing;

        public ConfigureATransport(IConfigureAnEndpoint configureEndpoint, T transport, Action<T> transportConfigurationAction)
        {
            Routing = configureEndpoint.Configuration.UseTransport(transport);
            transportConfigurationAction(transport);
            this.configureEndpoint = configureEndpoint;
        }

        public ConfigureATransport(IConfigureAnEndpoint configureEndpoint, T transport, IDefaultTransportConfiguration<T> defaultConfiguration, Action<T> transportConfigurationAction)
        {
            Routing = configureEndpoint.Configuration.UseTransport(transport);
            defaultConfiguration.ConfigureTransport(transport);
            transportConfigurationAction(transport);
            this.configureEndpoint = configureEndpoint;
        }

        public IConfigureAnEndpoint WithRouting<TDefault>() where TDefault : IDefaultRoutingConfiguration<T>, new()
        {
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureRouting(Routing);
            
            return configureEndpoint;
        }

        public IConfigureAnEndpoint WithRouting(Action<RoutingSettings<T>> routingConfigurationAction)
        {
            routingConfigurationAction(Routing);

            return configureEndpoint;
        }

        public IConfigureAnEndpoint WithRouting<TDefault>(Action<RoutingSettings<T>> routingConfigurationAction) where TDefault : IDefaultRoutingConfiguration<T>, new()
        {
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureRouting(Routing);
            routingConfigurationAction(Routing);
            
            return configureEndpoint;
        }

        
    }

}
