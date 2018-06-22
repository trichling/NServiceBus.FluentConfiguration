using System;
using NServiceBus.FluentConfiguration.Core;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{
    public class CoreConfiguration
    {
        [Fact]
        public void CanStartSimpleEndpoint() 
        {
            var endpointName = "Test";
            var schema = "mySchema";
            var connectionString = "";

            var endpoint = new ConfigureNServiceBus()
                .WithEndpoint(endpointName)
                .WithTransport<LearningTransport>(transport => { 
                    transport.StorageDirectory("./");
                })
                .WithRouting(routing => {
                })
                .WithPersistence<LearningPersistence>(persistence => {
                })
                .ManageEndpoint()
                .Start()
                .Instance;
            
            Assert.NotNull(endpoint);
        }
    }
}
