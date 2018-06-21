using System;

namespace NServiceBus.Configuration.WebApi
{

    public interface IDefaultEndpointConfiguration
    {

        void ConfigureEndpoint(EndpointConfiguration endpointConfiguration);

    }

}
