using NServiceBus.Serialization;

namespace NServiceBus.FluentConfiguration.Core
{
    public interface IDefaultSerializationConfiguration<T> where T : SerializationDefinition
    {
        void ConfigureSerialization(SerializationExtensions<T> serializationSettings);

    }

}