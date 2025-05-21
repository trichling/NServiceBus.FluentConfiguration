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

            var endpoint = new ConfigureNServiceBus()
                .WithEndpoint(endpointName)
                .WithTransport(new LearningTransport(), transport =>
                {
                    transport.StorageDirectory = "./";
                })
                .WithRouting(routing =>
                {
                })
                .WithPersistence<LearningPersistence>(persistence =>
                {
                })
                .WithSerialization<XmlSerializer>()
                .ManageEndpoint()
                .Start()
                .Instance;

            Assert.NotNull(endpoint);
        }
    }
}
