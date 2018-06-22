namespace NServiceBus.FluentConfiguration.Core
{

    public class ManageAnEndpoint : IManageAnEndpoint
    {
        private readonly EndpointConfiguration configuration;

        public ManageAnEndpoint(EndpointConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEndpointInstance Instance { get; private set; }

        public IManageAnEndpoint Start()
        {
            Instance = Endpoint.Start(configuration).GetAwaiter().GetResult();
            return this;
        }
    }

}