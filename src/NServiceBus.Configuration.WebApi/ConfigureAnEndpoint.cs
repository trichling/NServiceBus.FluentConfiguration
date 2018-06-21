using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.Configuration.WebApi
{

    public class ConfigureAnEndpoint : IConfigureAnEndpoint
    {
        private readonly IServiceCollection services;
        private readonly string name;
        private readonly IDefaultEndpointConfiguration defaultConfiguration;
        private readonly Action<EndpointConfiguration> endpointConfigurationAction;

        public ConfigureAnEndpoint(IServiceCollection services, string name)
        {
            this.services = services;
            this.name = name;

                Configuration = new EndpointConfiguration(name);
            }

        public ConfigureAnEndpoint(IServiceCollection services, string name, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            this.services = services;
            this.name = name;
            this.endpointConfigurationAction = endpointConfigurationAction;

            Configuration = new EndpointConfiguration(name);
            endpointConfigurationAction(Configuration);
        }

        public ConfigureAnEndpoint(IServiceCollection services, string name, IDefaultEndpointConfiguration defaultConfiguration, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            this.services = services;
            this.name = name;
            this.defaultConfiguration = defaultConfiguration;
            this.endpointConfigurationAction = endpointConfigurationAction;

            Configuration = new EndpointConfiguration(name);
            defaultConfiguration.ConfigureEndpoint(Configuration);
            endpointConfigurationAction(Configuration);
        }

        public EndpointConfiguration Configuration { get; private set; }       

        public IConfigureATransport<T> WithTransport<T>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new()
        {
            return new ConfigureATransport<T>(this, transportConfigurationAction);                        
        }

        public IConfigureATransport<T> WithTransport<T, TDefault>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new()
        {
           return new ConfigureATransport<T>(this, new TDefault(), transportConfigurationAction);       
        }

         public IConfigureAnEndpoint WithPersistence<T>(Action<PersistenceExtensions<T>> persistenceConfigurationAction) where T : PersistenceDefinition
        {
            var persistence = Configuration.UsePersistence<T>();
            persistenceConfigurationAction(persistence);
            return this;
        }

        public IConfigureAnEndpoint WithPersistence<T, TDefault>(Action<PersistenceExtensions<T>> persistenceConfigurationAction)
            where T : PersistenceDefinition
            where TDefault : IDefaultPersistenceConfiguration<T>, new()
        {
            var persistence = Configuration.UsePersistence<T>();
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigurePersistence(persistence);
            persistenceConfigurationAction(persistence);

            return this;
        }

        public void Start()
        {
            var instance = Endpoint.Start(Configuration).GetAwaiter().GetResult();
            services.AddSingleton<IMessageSession>(instance);
        }
    }

}