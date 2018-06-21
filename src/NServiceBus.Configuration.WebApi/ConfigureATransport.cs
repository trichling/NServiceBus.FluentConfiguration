using System;
using NServiceBus.Transport;

namespace NServiceBus.Configuration.WebApi
{
    public class ConfigureATransport<T> : IConfigureATransport<T> where T : TransportDefinition, new()
    {
        private readonly IConfigureAnEndpoint configureEndpoint;
        private Action<TransportExtensions<T>> transportConfigurationAction;

        public ConfigureATransport(IConfigureAnEndpoint configureEndpoint, Action<TransportExtensions<T>> transportConfigurationAction)
        {
            Transport = configureEndpoint.Configuration.UseTransport<T>();
            transportConfigurationAction(Transport);
        }

        public ConfigureATransport(IConfigureAnEndpoint configureEndpoint, IDefaultTransportConfiguration<T> defaultConfiguration, Action<TransportExtensions<T>> transportConfigurationAction)
        {
            Transport = configureEndpoint.Configuration.UseTransport<T>();
            defaultConfiguration.ConfigureTransport(Transport);
            transportConfigurationAction(Transport);
            this.configureEndpoint = configureEndpoint;
        }

        public TransportExtensions<T> Transport { get; private set; }

        public IConfigureAnEndpoint WithRouting(Action<RoutingSettings<T>> routingConfigurationAction)
        {
            var routing = Transport.Routing();
            routingConfigurationAction(routing);

            return configureEndpoint;
        }

        public IConfigureAnEndpoint WithRouting<TDefault>(Action<RoutingSettings<T>> routingConfigurationAction) where TDefault : IDefaultRoutingConfiguration<T>, new()
        {
            var routing = Transport.Routing();
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureRouting(routing);
            routingConfigurationAction(routing);
            
            return configureEndpoint;
        }


    }

}
