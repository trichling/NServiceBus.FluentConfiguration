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
           SqlServerTransportAndPersistenceProfile.IsApplied = false;
           ConventionProfile.IsApplied = false;
        }

        [Fact]
        public void ConfigurationProfile_SignatureTest()
        {
            new ConfigureNServiceBus()
                    .WithEndpoint("Test")
                    .WithConfiguration(new SqlServerTransportAndPersistenceProfile("datasource = blablabla"));

            Assert.True(SqlServerTransportAndPersistenceProfile.IsApplied);
        }

         [Fact]
        public void ConfigurationProfile_CanApplyMultipleProfiles()
        {
            new ConfigureNServiceBus()
                    .WithEndpoint("Test")
                    .WithConfiguration(new SqlServerTransportAndPersistenceProfile("datasource = blablabla"))
                    .WithConfiguration(new ConventionProfile());

            Assert.True(SqlServerTransportAndPersistenceProfile.IsApplied);
            Assert.True(ConventionProfile.IsApplied);
        }

        private class SqlServerTransportAndPersistenceProfile : IEndpointConfigurationProfile
        {
            public static bool IsApplied = false;
            private readonly string connectionString;

            public SqlServerTransportAndPersistenceProfile(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public void ApplyTo(IConfigureAnEndpoint configuration)
            {
                IsApplied = true;
                configuration
                    .WithPersistence<SqlPersistence>(cfg => cfg.ConnectionBuilder(() => new SqlConnection(connectionString)))
                    .WithTransport(new SqlServerTransport(connectionString), cfg => 
                    {
                    });
            }
        }

        private class ConventionProfile : IEndpointConfigurationProfile
        {
            public static bool IsApplied = false;

            public void ApplyTo(IConfigureAnEndpoint endpointConfiguration)
            {
                IsApplied = true;
                endpointConfiguration.WithConventions(cfg => {
                    cfg.DefiningMessagesAs(c => c.Namespace.StartsWith("Contracts"));
                });
            }
        }
    }
}