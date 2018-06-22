using NServiceBus.FluentConfiguration.Core;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{

    public class ConfigureAnEndpointTests
    {
        public ConfigureAnEndpointTests()
        {
            DefaultTransportConfiguration.ConfigureTransportCalled = false;
            DefaultPersistenceConfiguration.ConfigurePersistenceCalled = false;
        }

        [Fact]
        public void ConfigureAnEndpoint_WithTransport_ReturnsIConfigureATransport()
        {
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport<LearningTransport>();

            Assert.IsAssignableFrom(typeof(IConfigureATransport<LearningTransport>), configureTransport);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithTransport_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configuconfigureTransportreEndpoint = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport<LearningTransport>(cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport<LearningTransport, DefaultTransportConfiguration>();

            Assert.True(DefaultTransportConfiguration.ConfigureTransportCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport<LearningTransport, DefaultTransportConfiguration>(cfg => { configurationCallbackCalled = true; });

            Assert.True(DefaultTransportConfiguration.ConfigureTransportCalled);
            Assert.True(configurationCallbackCalled);
        }

        private class DefaultTransportConfiguration : IDefaultTransportConfiguration<LearningTransport>
        {
            public static bool ConfigureTransportCalled = false;

            public void ConfigureTransport(TransportExtensions<LearningTransport> transport)
            {
                ConfigureTransportCalled = true;
            }
        }

        [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ReturnsIConfigureAnEndpoint()
        {
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence>();

            Assert.IsAssignableFrom(typeof(IConfigureAnEndpoint), configurePersistence);
        }

          [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence>(cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void onfigureAnEndpoint_WithPersistence_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence, DefaultPersistenceConfiguration>();

            Assert.True(DefaultPersistenceConfiguration.ConfigurePersistenceCalled);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence, DefaultPersistenceConfiguration>(cfg => { configurationCallbackCalled = true; });

            Assert.True(DefaultPersistenceConfiguration.ConfigurePersistenceCalled);
            Assert.True(configurationCallbackCalled);
        }

        private class DefaultPersistenceConfiguration : IDefaultPersistenceConfiguration<LearningPersistence>
        {
            public static bool ConfigurePersistenceCalled = false;

            public void ConfigurePersistence(PersistenceExtensions<LearningPersistence> persistenceConfiguration)
            {
                ConfigurePersistenceCalled = true;
            }
        }

        [Fact]
        public void ConfigureEndpoint_ManageEndpoint_ReturnsIManageAnEndpoint()
        {
            var manageEndpoint = new ConfigureNServiceBus().WithEndpoint("Test").ManageEndpoint();

            Assert.IsAssignableFrom(typeof(IManageAnEndpoint), manageEndpoint);
        }
    }
  
}

   
