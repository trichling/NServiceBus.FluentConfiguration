using System;
using NServiceBus.Container;
using NServiceBus.ObjectBuilder;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IConfigureDependecyInjection
    {
        IConfigureDependecyInjection WithContainer<T>() where T : ContainerDefinition, new();

        IConfigureAnEndpoint WithComponents(Action<IConfigureComponents> componentsConfigurationAction);

    }
   
}
