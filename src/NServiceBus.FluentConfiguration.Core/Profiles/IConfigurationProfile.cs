namespace NServiceBus.FluentConfiguration.Core.Profiles
{

    public interface IConfigurationProfile
    {

        void ApplyTo(IConfigureAnEndpoint endpointConfiguration);
        
    }

}