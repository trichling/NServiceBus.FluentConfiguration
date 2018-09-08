using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Container;

namespace NServiceBus.FluentConfiguration.Core.DependencyInjection
{

    public static class ServiceCollectionExtensions
    {

        public static void ExistingServiceCollection(this ContainerCustomizations customizations, IServiceCollection serviceCollection)
        {
            customizations.Settings.Set<ServiceCollectoinBuilder.ServiceCollectionHolder>(new ServiceCollectoinBuilder.ServiceCollectionHolder(serviceCollection));
        }

    }

}