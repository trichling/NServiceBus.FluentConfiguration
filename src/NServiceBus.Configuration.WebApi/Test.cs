using Microsoft.Extensions.DependencyInjection;
using System;
using NServiceBus.Transport;
using NServiceBus.Transport.SQLServer;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;

namespace NServiceBus.Configuration.WebApi
{

    public class Test
    {

        public void HowDoesItLookLike() 
        {
            IServiceCollection x = null;

            var schema = "apetitoTableguestApi";
            var connectionString = "";

            x.AddNServiceBus()
                .WithEndpoint<DefaultEndpointConfiguration>("Test")
                .WithTransport<SqlServerTransport, DefaultSqlServerTransportConfiguration>(transport => { 
                    transport.DefaultSchema(schema);
                    transport.UseSchemaForEndpoint("apetito.TableGuest.BusinessComponents", "apetitoTableGuestBusinessComponents");
                })
                .WithRouting(routing => {
                    routing.RouteToEndpoint(typeof(object), "apetito.TableGuest.BusinessComponents");
                })
                .WithPersistence<SqlPersistence, DefaultSqlPersistenceConfiguration>(persistence => {
                    var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                    dialect.Schema(schema);
                    persistence.ConnectionBuilder(
                        connectionBuilder: () => {
                            return new SqlConnection(connectionString);
                        });
                    
                })
                .Start();
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