# NServiceBus.FluentConfiguration

_[![NuGet version](https://img.shields.io/nuget/v/NServiceBus.FluentConfiguration.Core.svg?style=flat)](https://www.nuget.org/packages/NServiceBus.FluentConfiguration.Core)_
_[![NuGet version](https://img.shields.io/nuget/v/NServiceBus.FluentConfiguration.WebApi.svg?style=flat)](https://www.nuget.org/packages/NServiceBus.FluentConfiguration.WebApi)_

Provides a fluent API to configure NServiceBus endpoints

Configuring an NServiceBus endpoint hosted in a .NET Core API happens in ConfigureServices, but the code you have to write for this does not fit nicely into the serives.AddNServiceBus schema of .NET Core. Instead one has to write code that looks alien to .NET Core WebApi.
Using NServiceBus.FluentConfiguration.WebApi this gap is filled by providing an nativly looking fluent API starting from the IServiceCollection using the AddNServiceBus extension method. From there a fluent API guides you thorugh the steps to configure an start an endpoint.
While this is only a thin wrapper over the native NServiceBus-Configuration API, it provides the additional mechanism to exclude configuration that is shared among multiple endpoints into seperate classes and only to endpoint specific configuration within the endpoint.
Using the dependent NServiceBus.FluentConfiguration.Core project the fluent API can be used for console apps and the like as well.

## Example

This is what the configuration looks like for an endpoint using SqlServer transport and Sql Persistence.

``` CSharp
var connectionString = Configuration.GetConnectionString("MyConnString");
var endpointName = "someName"
var schema = "someNameSchema";

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();

var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.ConnectionString(connectionString);
transport.DefaultSchema(schema);
transport.UseSchemaForQueue("error", "dbo");
transport.UseSchemaForQueue("audit", "dbo");
transport.UseSchemaForEndpoint(endpointName, schema);
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

var routing = transport.Routing();
routing.RouteToEndpoint(typeof(SomeCommand), schema);

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
dialect.Schema(schema);
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new SqlConnection(connectionString);
    });
persistence.TablePrefix("");
var subscriptions = persistence.SubscriptionSettings();
subscriptions.CacheFor(TimeSpan.FromMinutes(1));

var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
services.AddSingleton<IEndpointInstance>(endpointInstance);
```

Using NServiceBus.FluentConfiguration.WebApi this can be rewritten as:

``` CSharp
var connectionString = Configuration.GetConnectionString("MyConnString");
var endpointName = "someName"
var schema = "someNameSchema";

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

    })
    .ManageEndpoint()
    .Start();
```

## Going through the code

The configuration process now starts with the IServiceCollection as it is usual in ConfigureServices() method. The entry point to the configuration is the AddNServiceBus extension method. From there you can configure the major topics:

- General Endpoint settings
- Transport
- Routing
- Persistence

The generic parameters like `DefaultEndpointConfiguration` refer to classes implementing a certain interface to scope out general configurations that apply to multiple endpoints:

``` CSharp
public class DefaultEndpointConfiguration : IDefaultEndpointConfiguration
{
    public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();
    }
}
```

This configuration - if any - is applied first. Afterwards the given configuration callback is called, that allows for endpoint specific configuration. An example is configuration that depends on appsettings like connection strings.

The ManageEndpoint() method allows to start the endpoint. In case of NServiceBus.FluentConfiguration.WebApi the resulting IEndpointInstance is registered in the DI container as a singleton.

# Configuration Profiles

As of version 1.1.0 the library supports using configuration profiles by providing a class that implements ```IEndpointConfigurationProfile```. A profile is a part of an endpoint configuration. This allows for a more modular structure for your configurations. Say you use Sql Server transport & persistence in the same database. You can create a profile class for that:

``` CSharp
public class SqlServerTransportAndPersistenceProfile : IEndpointConfigurationProfile
{
    private readonly string connectionString;

    public SqlServerTransportAndPersistenceProfile(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void ApplyTo(IConfigureAnEndpoint endpointConfiguration)
    {
        endpointConfiguration
            .WithPersistence<SqlPersistence>(cfg =>
            {
                cfg.SqlDialect<SqlDialect.MsSqlServer>();
                cfg.ConnectionBuilder(() => new SqlConnection(connectionString));
            })
            .WithTransport<SqlServerTransport>(cfg =>
            {
                cfg.ConnectionString(connectionString);
                cfg.UseSchemaForQueue("error", "dbo");
                cfg.UseSchemaForQueue("audit", "dbo");
                cfg.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            });
    }
}
```

A profile can be used by calling the ```WithConfiguration``` method on the ```IConfiguraAnEndpoint``` interface:

``` CSharp
 var endpointConfiguration = configuration.WithEndpoint(endpointName)
    .WithConfiguration(new SqlServerTransportAndPersistenceProfile(connectionString))
    .WithConfiguration(new DefaultConventionsProfile())
    .WithConfiguration(new DefaultAuditAndMetricProfile())
```

The method can be called multiple times and can be mixed and matched with all other configuration options.

# Known issues

This project was started as to make life easier when configuring an NSercieBus endpoint within a WebApi. It is perfectly possible that not the whole configuration options that NServiceBus makes available are available through the fluent API. If you find something is missing, I am happy to receive pull requests.
