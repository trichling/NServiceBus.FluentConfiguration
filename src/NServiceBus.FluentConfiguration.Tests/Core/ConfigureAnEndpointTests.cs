using System.Data.SqlClient;
using NServiceBus.FluentConfiguration.Core;
using NServiceBus.FluentConfiguration.Core.Profiles;
using NServiceBus.Persistence.Sql;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{

    public class ConfigureAnEndpointTests
    {
        public ConfigureAnEndpointTests()
        {
            ExampleConfigurationProfile.IsApplied = false;
            DefaultEndpointConfiguration.ConfigureEndpointCalled = false;
            DefaultTransportConfiguration.ConfigureTransportCalled = false;
            DefaultPersistenceConfiguration.ConfigurePersistenceCalled = false;
            DefaultConventinosConfiguration.ConfigurePersistenceCalled = false;
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_WithConfiguration_ProvidingConfigurationProfile_ConfigurationProfileIsApplied()
        {
            var configureEndpointWithConfiguration = new ConfigureNServiceBus().WithEndpoint("Test")
                .WithConfiguration(new ExampleConfigurationProfile("some parameter"));

            Assert.True(ExampleConfigurationProfile.IsApplied);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_WithConfiguration_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configureEndpointWithConfiguration = new ConfigureNServiceBus().WithEndpoint("Test")
                .WithConfiguration<DefaultEndpointConfiguration>();

            Assert.True(DefaultEndpointConfiguration.ConfigureEndpointCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_WithConfiguration_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureEndpointWithConfiguration = new ConfigureNServiceBus().WithEndpoint("Test")
                .WithConfiguration(cfg => configurationCallbackCalled = true);

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_WithConfiguration_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureEndpointWithConfiguration = new ConfigureNServiceBus().WithEndpoint("Test")
                .WithConfiguration<DefaultEndpointConfiguration>(cfg => configurationCallbackCalled = true);

            Assert.True(configurationCallbackCalled);
            Assert.True(DefaultEndpointConfiguration.ConfigureEndpointCalled);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithTransport_ReturnsIConfigureATransport()
        {
            var configureTransport = new ConfigureNServiceBus()
                .WithEndpoint("Test")
                .WithTransport(new LearningTransport());

            Assert.IsAssignableFrom<IConfigureATransport<LearningTransport>>(configureTransport);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithTransport_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configuconfigureTransportreEndpoint = new ConfigureNServiceBus()
                .WithEndpoint("Test")
                .WithTransport(new LearningTransport(), cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configureTransport = new ConfigureNServiceBus()
                .WithEndpoint("Test")
                .WithTransport<LearningTransport, DefaultTransportConfiguration>(new LearningTransport());

            Assert.True(DefaultTransportConfiguration.ConfigureTransportCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureTransport = new ConfigureNServiceBus()
                .WithEndpoint("Test")
                .WithTransport<LearningTransport, DefaultTransportConfiguration>(new LearningTransport(), cfg => { configurationCallbackCalled = true; });

            Assert.True(DefaultTransportConfiguration.ConfigureTransportCalled);
            Assert.True(configurationCallbackCalled);
        }

        private class DefaultTransportConfiguration : IDefaultTransportConfiguration<LearningTransport>
        {
            public static bool ConfigureTransportCalled = false;

            public void ConfigureTransport(LearningTransport transport)
            {
                ConfigureTransportCalled = true;
            }
        }

        [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ReturnsIConfigureAnEndpoint()
        {
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence>();

            Assert.IsAssignableFrom<IConfigureAnEndpoint>(configurePersistence);
        }

          [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithPersistence<LearningPersistence>(cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithPersistence_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
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

        [Fact]
        public void ConfigureAnEndpoint_WithConventions_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithConventions(cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithConventions_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithConventions<DefaultConventinosConfiguration>();

            Assert.True(DefaultConventinosConfiguration.ConfigurePersistenceCalled);
        }

        [Fact]
        public void ConfigureAnEndpoint_WithConventions_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configurePersistence = new ConfigureNServiceBus().WithEndpoint("Test").WithConventions<DefaultConventinosConfiguration>(cfg => { configurationCallbackCalled = true ;});

            Assert.True(DefaultConventinosConfiguration.ConfigurePersistenceCalled);
            Assert.True(configurationCallbackCalled);
        }

        private class DefaultEndpointConfiguration : IDefaultEndpointConfiguration
        {
            public static bool ConfigureEndpointCalled = false;

            public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
            {
                ConfigureEndpointCalled = true;
            }
        }

        private class ExampleConfigurationProfile : IEndpointConfigurationProfile
        {
            public static bool IsApplied = false;

            public ExampleConfigurationProfile(string someParameter)
            {
            }

            public void ApplyTo(IConfigureAnEndpoint configuration)
            {
                IsApplied = true;
                configuration
                    .WithPersistence<LearningPersistence>()
                    .WithTransport<LearningTransport>(new LearningTransport());
            }
        }

        private class DefaultConventinosConfiguration : IDefaultConventionsConfiguration
        {
            public static bool ConfigurePersistenceCalled = false;

            public void ConfigureConventions(ConventionsBuilder conventionsConfiguration)
            {
                ConfigurePersistenceCalled = true;
            }
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

            Assert.IsAssignableFrom<IManageAnEndpoint>(manageEndpoint);
        }

       
    }
  
}

   
