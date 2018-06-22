using NServiceBus.FluentConfiguration.Core;
using NServiceBus.FluentConfiguration.WebApi;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class NServiceBusConfigurationServiceCollectionExtensions
    {

        public static IConfigureNServiceBus AddNServiceBus(this IServiceCollection services) 
        {
            return new NServiceBus.FluentConfiguration.WebApi.ConfigureNServiceBus(services);
        }

    }

}