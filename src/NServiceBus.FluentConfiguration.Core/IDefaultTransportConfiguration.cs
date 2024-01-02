using System;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{
    public interface IDefaultTransportConfiguration<T> where T : TransportDefinition
    {

        void ConfigureTransport(T transport);

    }
}