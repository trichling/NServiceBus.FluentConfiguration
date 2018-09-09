using System;
using NServiceBus.FluentConfiguration.Core.Profiles;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public class ConfigureAnEndpoint : IConfigureAnEndpoint
    {
        private readonly string name;
        private readonly IDefaultEndpointConfiguration defaultConfiguration;
        private readonly Action<EndpointConfiguration> endpointConfigurationAction;

        internal ConfigureAnEndpoint(EndpointConfiguration existingConfiguration)
        {
            Configuration = existingConfiguration;
        }

        public ConfigureAnEndpoint(string name)
        {
            this.name = name;

            Configuration = new EndpointConfiguration(name);
        }

        public ConfigureAnEndpoint(string name, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            this.name = name;
            this.endpointConfigurationAction = endpointConfigurationAction;

            Configuration = new EndpointConfiguration(name);
            endpointConfigurationAction(Configuration);
        }

        public ConfigureAnEndpoint(string name, IDefaultEndpointConfiguration defaultConfiguration, Action<EndpointConfiguration> endpointConfigurationAction)
        {
            this.name = name;
            this.defaultConfiguration = defaultConfiguration;
            this.endpointConfigurationAction = endpointConfigurationAction;

            Configuration = new EndpointConfiguration(name);
            defaultConfiguration.ConfigureEndpoint(Configuration);
            endpointConfigurationAction(Configuration);
        }

        public EndpointConfiguration Configuration { get; private set; }       

        public IConfigureAnEndpoint WithConfiguration(IConfigurationProfile profile)
        {
            profile.ApplyTo(this);
            return this;
        }

        public IConfigureAnEndpoint WithConfiguration(Action<EndpointConfiguration> endpointConfigurationAction)
        {
            endpointConfigurationAction(Configuration);
            return this;
        }

        public IConfigureAnEndpoint WithConfiguration<TDefault>() where TDefault : IDefaultEndpointConfiguration, new()
        {
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureEndpoint(Configuration);
            return this;
        }

        public IConfigureAnEndpoint WithConfiguration<TDefault>(Action<EndpointConfiguration> endpointConfigurationAction) where TDefault : IDefaultEndpointConfiguration, new()
        {
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureEndpoint(Configuration);
            endpointConfigurationAction(Configuration);
            return this;
        }


        public IConfigureATransport<T> WithTransport<T>() where T : TransportDefinition, new()
        {
           return new ConfigureATransport<T>(this, t => {});       
        }   

        public IConfigureATransport<T> WithTransport<T, TDefault>() where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new()
        {
           return new ConfigureATransport<T>(this, new TDefault(), t => {});       
        }

        public IConfigureATransport<T> WithTransport<T>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new()
        {
            return new ConfigureATransport<T>(this, transportConfigurationAction);                        
        }

        public IConfigureATransport<T> WithTransport<T, TDefault>(Action<TransportExtensions<T>> transportConfigurationAction) where T : TransportDefinition, new() where TDefault : IDefaultTransportConfiguration<T>, new()
        {
           return new ConfigureATransport<T>(this, new TDefault(), transportConfigurationAction);       
        }

        public IConfigureAnEndpoint WithPersistence<T>() where T : PersistenceDefinition
        {
            var persistence = Configuration.UsePersistence<T>();
            return this;
        }

        public IConfigureAnEndpoint WithPersistence<T, TDefault>()
            where T : PersistenceDefinition
            where TDefault : IDefaultPersistenceConfiguration<T>, new()
        {
            var persistence = Configuration.UsePersistence<T>();
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigurePersistence(persistence);

            return this;
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

        public IConfigureAnEndpoint WithConventions(Action<ConventionsBuilder> conventionConfigurationAction)
        {
            var conventions = Configuration.Conventions();
            conventionConfigurationAction(conventions);
            
            return this;
        }

        public IConfigureAnEndpoint WithConventions<TDefault>() where TDefault : IDefaultConventionsConfiguration, new()
        {
            var conventions = Configuration.Conventions();
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureConventions(conventions);
            
            return this;
        }

        public IConfigureAnEndpoint WithConventions<TDefault>(Action<ConventionsBuilder> conventionConfigurationAction) where TDefault : IDefaultConventionsConfiguration, new()
        {
            var conventions = Configuration.Conventions();
            var defaultConfiguration = new TDefault();
            defaultConfiguration.ConfigureConventions(conventions);
            conventionConfigurationAction(conventions);

            return this;
        }

        public virtual IManageAnEndpoint ManageEndpoint()
        {
            return new ManageAnEndpoint(Configuration);
        }

       
    }

}