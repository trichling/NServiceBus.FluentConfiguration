using NServiceBus.Persistence;

namespace NServiceBus.Configuration.WebApi
{
    public interface IDefaultPersistenceConfiguration<T> where T : PersistenceDefinition
    {

        void ConfigurePersistence(PersistenceExtensions<T> persistenceConfiguration);

    }
}
