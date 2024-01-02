using NServiceBus.FluentConfiguration.Core;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{

    public class ConfigureATransportTests
    {

        public ConfigureATransportTests()
        {
            DefaultRoutingConfiguration.ConfigureRoutingCalled = false;
        }

        [Fact]
        public void ConfigureATransport_WithRouting_ReturnsIConfigureAnEndpoint()
        {
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport(new LearningTransport()).WithRouting(r => {});

            Assert.IsAssignableFrom(typeof(IConfigureAnEndpoint), configureTransport);
        }



         [Fact]
        public void ConfigureATransport_WithRouting_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport(new LearningTransport()).WithRouting(r => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

         [Fact]
        public void ConfigureATransport_WithRouting_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport(new LearningTransport()).WithRouting<DefaultRoutingConfiguration>();

            Assert.True(DefaultRoutingConfiguration.ConfigureRoutingCalled);
        }

        [Fact]
        public void ConfigureATransport_WithRouting_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureTransport = new ConfigureNServiceBus().WithEndpoint("Test").WithTransport(new LearningTransport()).WithRouting<DefaultRoutingConfiguration>(cfg => { configurationCallbackCalled = true; });

            Assert.True(DefaultRoutingConfiguration.ConfigureRoutingCalled);
            Assert.True(configurationCallbackCalled);
        }

         private class DefaultRoutingConfiguration : IDefaultRoutingConfiguration<LearningTransport>
        {
            public static bool ConfigureRoutingCalled = true;
            public void ConfigureRouting(RoutingSettings<LearningTransport> routingSettings)
            {
                ConfigureRoutingCalled = true;
            }
        }
    }

   
}