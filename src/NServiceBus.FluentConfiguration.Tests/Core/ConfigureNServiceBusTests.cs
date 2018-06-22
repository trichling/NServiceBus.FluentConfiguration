using NServiceBus.FluentConfiguration.Core;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{

    public class NServiceBusConfigurationTests
    {

        public NServiceBusConfigurationTests()
        {
            DefaultEndpointConfiguration.ConfigureEndpointCalled = false;
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingName_ReturnsIConfigureAnEndpoint()
        {
            var configureEndpoint = new ConfigureNServiceBus().WithEndpoint("Test");

            Assert.IsAssignableFrom(typeof(IConfigureAnEndpoint), configureEndpoint);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingConfigurationCallback_ConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureEndpoint = new ConfigureNServiceBus().WithEndpoint("Test", cfg => { configurationCallbackCalled = true; });

            Assert.True(configurationCallbackCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfiguration_DefaultConfigurationIsApplied()
        {
            var configureEndpoint = new ConfigureNServiceBus().WithEndpoint<DefaultEndpointConfiguration>("Test");

            Assert.True(DefaultEndpointConfiguration.ConfigureEndpointCalled);
        }

        [Fact]
        public void ConfigureNServiceBus_WithEndpoint_ProvidingDefaultConfigurationAndConfigurationCallback_DefaultConfigurationIsAppliedAndConfigurationCallbackIsCalled()
        {
            var configurationCallbackCalled = false;
            var configureEndpoint = new ConfigureNServiceBus().WithEndpoint<DefaultEndpointConfiguration>("Test", cfg => { configurationCallbackCalled = true; });

            Assert.True(DefaultEndpointConfiguration.ConfigureEndpointCalled);
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
    }

}