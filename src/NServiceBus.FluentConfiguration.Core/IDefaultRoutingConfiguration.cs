using System;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{
    public interface IDefaultRoutingConfiguration<T> where T : TransportDefinition, new()
    {

        void ConfigureRouting(RoutingSettings<T> routingSettings) ;
        
    }
}