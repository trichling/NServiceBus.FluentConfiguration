using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.FluentConfiguration.Core;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;
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
            var schema = "mySchema";
            var connectionString = "";

            services.AddNServiceBus()
                .WithEndpoint<DefaultEndpointConfiguration>(endpointName)
                .WithTransport<LearningTransport>(transport => { 
                    transport.StorageDirectory("./");
                })
                .WithRouting(routing => {
                })
                .WithPersistence<LearningPersistence>(persistence => {
                })
                .ManageEndpoint()
                .Start();

            var serviceProvider = services.BuildServiceProvider();
            var endpoint = serviceProvider.GetService(typeof(IEndpointInstance));
            
            Assert.NotNull(endpoint);
        }

        [Fact]
        public void CanConfigureAnEndpointWithSqlTransportAndPersistence() 
        {
            IServiceCollection services = new ServiceCollection(); // null; //new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            var endpointName = "Test";
            var schema = "mySchema";
            var connectionString = "";

            services.AddNServiceBus()
                .WithEndpoint<DefaultEndpointConfiguration>(endpointName)
                .WithTransport<SqlServerTransport, DefaultSqlServerTransportConfiguration>(transport => { 
                    transport.DefaultSchema(schema);
                    transport.UseSchemaForEndpoint(endpointName, endpointName);
                })
                .WithRouting(routing => {
                    routing.RouteToEndpoint(typeof(object), endpointName);
                })
                .WithPersistence<SqlPersistence, DefaultSqlPersistenceConfiguration>(persistence => {
                    var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                    dialect.Schema(schema);
                    persistence.ConnectionBuilder(
                        connectionBuilder: () => {
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

    public class DefaultSqlServerTransportConfiguration : IDefaultTransportConfiguration<SqlServerTransport>
    {

        public void ConfigureTransport(TransportExtensions<SqlServerTransport> transport)
        {
            transport.UseSchemaForQueue("error", "dbo");
            transport.UseSchemaForQueue("audit", "dbo");
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        }
    }

    public class DefaultSqlPersistenceConfiguration : IDefaultPersistenceConfiguration<SqlPersistence>
    {
        public void ConfigurePersistence(PersistenceExtensions<SqlPersistence> persistence)
        {
            persistence.TablePrefix("");
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        }
    }
}