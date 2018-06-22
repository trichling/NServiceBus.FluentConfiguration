using System;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{
    public interface IDefaultTransportConfiguration<T> where T : TransportDefinition, new()
    {

        void ConfigureTransport(TransportExtensions<T> transport);

    }
}