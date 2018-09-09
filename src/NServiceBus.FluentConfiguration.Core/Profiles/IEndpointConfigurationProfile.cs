namespace NServiceBus.FluentConfiguration.Core.Profiles
{

    public interface IEndpointConfigurationProfile
    {

        void ApplyTo(IConfigureAnEndpoint endpointConfiguration);
        
    }

}