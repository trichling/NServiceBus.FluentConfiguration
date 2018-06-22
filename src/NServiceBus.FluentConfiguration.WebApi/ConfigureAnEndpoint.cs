using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.FluentConfiguration.Core;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.WebApi
{

    public class ConfigureAnEndpoint : Core.ConfigureAnEndpoint
    {
        private readonly IServiceCollection services;

        public ConfigureAnEndpoint(IServiceCollection services, string name) : base(name)
        {
            this.services = services;
        }

        public ConfigureAnEndpoint(IServiceCollection services, string name, Action<EndpointConfiguration> endpointConfigurationAction)
            : base(name, endpointConfigurationAction)
        {
            this.services = services;
        }

        public ConfigureAnEndpoint(IServiceCollection services, string name, IDefaultEndpointConfiguration defaultConfiguration, Action<EndpointConfiguration> endpointConfigurationAction)
          : base(name, defaultConfiguration, endpointConfigurationAction)
        {
            this.services = services;
        }

        public override IManageAnEndpoint ManageEndpoint()
        {
            return new ManageAnEndpoint(services, Configuration);
        }
    }

}