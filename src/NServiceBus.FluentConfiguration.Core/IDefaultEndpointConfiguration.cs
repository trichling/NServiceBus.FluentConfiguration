using System;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IDefaultEndpointConfiguration
    {

        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration);

    }

}
