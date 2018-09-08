using System;
using NServiceBus.Container;
using NServiceBus.ObjectBuilder;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public class ConfigureDependencyInjection : IConfigureDependecyInjection
    {
        private readonly IConfigureAnEndpoint configureEndpoint;

        public ConfigureDependencyInjection(IConfigureAnEndpoint configureEndpoint)
        {
            this.configureEndpoint = configureEndpoint;
        }

        public IConfigureDependecyInjection WithContainer<T>() where T : ContainerDefinition, new()
        {
            configureEndpoint.Configuration.UseContainer<T>();

            return this;
        }

        public IConfigureAnEndpoint WithComponents(Action<IConfigureComponents> componentsConfigurationAction)
        {
            configureEndpoint.Configuration.RegisterComponents(componentsConfigurationAction);

            return configureEndpoint;
        }

        
    }

}
