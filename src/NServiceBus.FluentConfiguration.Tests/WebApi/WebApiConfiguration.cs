using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.FluentConfiguration.Core;
using Xunit;

namespace NServiceBus.FluentConfiguration.Tests
{
    public class WebApiConfiguration
    {
        [Fact]
        public void EndpointGetsRegisteredInDIContainer()
        {
            IServiceCollection services = new ServiceCollection(); // null; //new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            var endpointName = "Test";

            services
                .AddNServiceBus()
                .WithEndpoint<DefaultEndpointConfiguration>(endpointName)
                .WithTransport<LearningTransport>(
                    new LearningTransport(),
                    transport =>
                    {
                        transport.StorageDirectory = "./";
                    }
                )
                .WithRouting(routing => { })
                .WithPersistence<LearningPersistence>(persistence => { })
                .WithSerialization<XmlSerializer>()
                .ManageEndpoint()
                .Start();

            var serviceProvider = services.BuildServiceProvider();
            var endpoint = serviceProvider.GetService(typeof(IEndpointInstance));

            Assert.NotNull(endpoint);
        }

        [Fact]
        public void CanConfigureAnEndpointWithSqlTransportAndPersistence()
        {
            var services = new ServiceCollection();

            var endpointName = "Test";
            var schema = "mySchema";
            var connectionString =
                "Server=localhost;Database=nservicebus;Trusted_Connection=True;MultipleActiveResultSets=true";

            services
                .AddNServiceBus()
                .WithEndpoint<DefaultEndpointConfiguration>(endpointName)
                .WithTransport<SqlServerTransport, DefaultSqlServerTransportConfiguration>(
                    new SqlServerTransport(connectionString),
                    transport =>
                    {
                        transport.DefaultSchema = schema;
                    }
                )
                .WithRouting(routing =>
                {
                    routing.RouteToEndpoint(typeof(object), endpointName);
                })
                .WithPersistence<SqlPersistence, DefaultSqlPersistenceConfiguration>(persistence =>
                {
                    var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                    dialect.Schema(schema);
                    persistence.ConnectionBuilder(connectionBuilder: () =>
                    {
                        return new SqlConnection(connectionString);
                    });
                });
        }
    }

    public class DefaultEndpointConfiguration : IDefaultEndpointConfiguration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableInstallers();
        }
    }

    public class DefaultSqlServerTransportConfiguration
        : IDefaultTransportConfiguration<SqlServerTransport>
    {
        public void ConfigureTransport(SqlServerTransport transport)
        {
            transport.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
            transport.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");
            transport.TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive;
        }
    }

    public class DefaultSqlPersistenceConfiguration
        : IDefaultPersistenceConfiguration<SqlPersistence>
    {
        public void ConfigurePersistence(PersistenceExtensions<SqlPersistence> persistence)
        {
            persistence.TablePrefix("");
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        }
    }
}
