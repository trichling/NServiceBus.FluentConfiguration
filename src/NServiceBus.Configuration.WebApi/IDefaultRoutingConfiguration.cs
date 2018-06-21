using System;
using NServiceBus.Transport;

namespace NServiceBus.Configuration.WebApi
{
    public interface IDefaultRoutingConfiguration<T> where T : TransportDefinition, new()
    {

        void ConfigureRouting(RoutingSettings<T> routingSettings) ;
        
    }
}