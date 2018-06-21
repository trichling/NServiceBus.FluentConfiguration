using NServiceBus.Configuration.WebApi;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class NServiceBusConfigurationServiceCollectionExtensions
    {

        public static IConfigureNServiceBus AddNServiceBus(this IServiceCollection services) 
        {
            return new ConfigureNServiceBus(services);
        }

    }

}