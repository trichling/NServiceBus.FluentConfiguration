using System.Data.SqlClient;
using NServiceBus.FluentConfiguration.Core;
using NServiceBus.FluentConfiguration.Core.Profiles;
using NServiceBus.Persistence.Sql;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{

    public class ConfigurationProfileTests
    {
        public ConfigurationProfileTests()
        {
           
        }

        [Fact]
        public void ConfigurationProfile_SignatureTest()
        {
            new ConfigureNServiceBus()
                    .WithEndpoint("Test")
                    .WithProfile(new SqlServerTransportAndPersistenceProfile("datasource = blablabla"));
        }

        private class SqlServerTransportAndPersistenceProfile : IConfigurationProfile
        {
            private readonly string connectionString;

            public SqlServerTransportAndPersistenceProfile(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public void ApplyTo(IConfigureAnEndpoint configuration)
            {
                configuration
                    .WithPersistence<SqlPersistence>(cfg => cfg.ConnectionBuilder(() => new SqlConnection(connectionString)))
                    .WithTransport<SqlServerTransport>(cfg => 
                    {
                        cfg.ConnectionString(connectionString);
                    });
            }
        }
    }
}