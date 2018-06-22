using System;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace NServiceBus.FluentConfiguration.Core
{

    public interface IManageAnEndpoint
    {

        IEndpointInstance Instance { get; }

        IManageAnEndpoint Start();
        
    }
}