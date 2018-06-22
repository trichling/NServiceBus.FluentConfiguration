using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.FluentConfiguration.Core;

namespace NServiceBus.FluentConfiguration.WebApi
{

    public class ConfigureNServiceBus : IConfigureNServiceBus
    {
        private IServiceCollection services;

        public ConfigureNServiceBus(IServiceCollection services)
        {
            this.services = services;
        }

        public IConfigureAnEndpoint WithEndpoint(string name)
        {
            return new ConfigureAnEndpoint(services, name);
        }

        public IConfigureAnEndpoint WithEndpoint<TDefault>(string name) where TDefault : IDefaultEndpointConfiguration, new()
        {
            return new ConfigureAnEndpoint(services, name, new TDefault(), p => {});
        }

        public IConfigureAnEndpoint WithEndpoint(string name, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            return new ConfigureAnEndpoint(services, name, endpointConfigurationAction);
        }

        public IConfigureAnEndpoint WithEndpoint<TDefault>(string name, Action<EndpointConfiguration> endpointConfigurationAction) where TDefault : IDefaultEndpointConfiguration, new()
        {
            return new ConfigureAnEndpoint(services, name, new TDefault(), endpointConfigurationAction);
        }


    }

}