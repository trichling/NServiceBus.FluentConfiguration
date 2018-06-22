using NServiceBus.Persistence;

namespace NServiceBus.FluentConfiguration.Core
{
    public interface IDefaultPersistenceConfiguration<T> where T : PersistenceDefinition
    {

        void ConfigurePersistence(PersistenceExtensions<T> persistenceConfiguration);

    }
}
