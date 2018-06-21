using System;
using NServiceBus.Transport;

namespace NServiceBus.Configuration.WebApi
{
    public interface IDefaultTransportConfiguration<T> where T : TransportDefinition, new()
    {

        void ConfigureTransport(TransportExtensions<T> transport);

    }
}