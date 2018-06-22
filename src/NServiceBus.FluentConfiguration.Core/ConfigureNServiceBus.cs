using System;

namespace NServiceBus.FluentConfiguration.Core
{

    public class ConfigureNServiceBus : IConfigureNServiceBus
    {

        public ConfigureNServiceBus()
        {
        }

        public IConfigureAnEndpoint WithEndpoint(string name)
        {
            return new ConfigureAnEndpoint(name);
        }

        public IConfigureAnEndpoint WithEndpoint<TDefault>(string name) where TDefault : IDefaultEndpointConfiguration, new()
        {
            return new ConfigureAnEndpoint(name, new TDefault(), p => {});
        }

        public IConfigureAnEndpoint WithEndpoint(string name, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            return new ConfigureAnEndpoint(name, endpointConfigurationAction);
        }

        public IConfigureAnEndpoint WithEndpoint<TDefault>(string name, Action<EndpointConfiguration> endpointConfigurationAction) where TDefault : IDefaultEndpointConfiguration, new()
        {
            return new ConfigureAnEndpoint(name, new TDefault(), endpointConfigurationAction);
        }


    }

}