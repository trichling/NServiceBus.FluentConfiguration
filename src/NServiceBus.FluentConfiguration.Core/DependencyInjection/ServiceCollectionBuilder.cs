using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Container;
using NServiceBus.ObjectBuilder.Common;
using NServiceBus.Settings;

namespace NServiceBus.FluentConfiguration.Core.DependencyInjection
{

    public class ServiceCollectoinBuilder : ContainerDefinition
    {
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            if (settings.TryGet(out ServiceCollectionHolder existingServiceCollection))
            {
                return new ServiceCollectionContainer(existingServiceCollection.Services);
            }

            return new ServiceCollectionContainer();
        }

        internal class ServiceCollectionHolder
        {
            public ServiceCollectionHolder(IServiceCollection services)
            {
                Services = services;
            }

            public IServiceCollection Services { get; }
        }
    }
}