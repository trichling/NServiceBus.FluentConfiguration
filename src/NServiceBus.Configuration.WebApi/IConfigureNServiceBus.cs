using System;

namespace NServiceBus.Configuration.WebApi
{

    public interface IConfigureNServiceBus 
    {

        IConfigureAnEndpoint WithEndpoint(string name);
        IConfigureAnEndpoint WithEndpoint(string name, Action<EndpointConfiguration> endpointConfigurationAction);
        IConfigureAnEndpoint WithEndpoint<TDefault>(string name) where TDefault : IDefaultEndpointConfiguration, new();
        IConfigureAnEndpoint WithEndpoint<TDefault>(string name, Action<EndpointConfiguration> endpointConfigurationAction) where TDefault : IDefaultEndpointConfiguration, new();

    }

  

}