using Microsoft.Extensions.DependencyInjection;
using NServiceBus.FluentConfiguration.Core;

namespace NServiceBus.FluentConfiguration.WebApi
{

    public class ManageAnEndpoint : IManageAnEndpoint
    {
        private readonly IServiceCollection services;
        private readonly EndpointConfiguration configuration;

        public ManageAnEndpoint(IServiceCollection services, EndpointConfiguration configuration)
        {
            this.services = services;
            this.configuration = configuration;
        }

        public IEndpointInstance Instance { get; private set; }

        public IManageAnEndpoint Start()
        {
            Instance = Endpoint.Start(configuration).GetAwaiter().GetResult();
            services.AddSingleton<IEndpointInstance>(Instance);
            return this;
        }
    }

}