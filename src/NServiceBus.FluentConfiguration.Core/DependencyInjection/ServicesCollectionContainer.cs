using System;
using System.Collections.Generic;
using NServiceBus.ObjectBuilder.Common;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace NServiceBus.FluentConfiguration.Core.DependencyInjection
{

    public class ServiceCollectionContainer : IContainer
    {
        private readonly IServiceCollection services;
        private ServiceProvider serviceProvider;

        public ServiceCollectionContainer()
            : this(new ServiceCollection())
        {
           
        }

        public ServiceCollectionContainer(IServiceCollection services)
        {
            this.services = services;
            this.serviceProvider = services.BuildServiceProvider();
        }

        public object Build(Type typeToBuild)
        {
            return serviceProvider.GetService(typeToBuild);
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            return serviceProvider.GetServices(typeToBuild);
        }

        public IContainer BuildChildContainer()
        {
            return new ServiceCollectionContainer(services);
        }

        public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
        {
            RegisterAllServices(component, dependencyLifecycle);
            serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterAllServices(Type component, DependencyLifecycle dependencyLifecycle)
        {
            var servicesOfT = GetAllServices(component);
            foreach (var service in servicesOfT)
            {
                switch (dependencyLifecycle)
                {
                    case DependencyLifecycle.InstancePerCall:
                        services.AddTransient(service, component);
                    break;
                    case DependencyLifecycle.InstancePerUnitOfWork:
                        services.AddScoped(service, component);
                    break;
                    case DependencyLifecycle.SingleInstance:
                        services.AddSingleton(service, component);
                    break;
                }
            }
        }

        public void Configure<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
            RegisterAllServices(component, dependencyLifecycle);
            serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterAllServices<T>(Func<T> component, DependencyLifecycle dependencyLifecycle)
        {
            var servicesOfT = GetAllServices<T>();
            foreach (var service in servicesOfT)
            {
                switch (dependencyLifecycle)
                {
                    case DependencyLifecycle.InstancePerCall:
                        services.AddTransient(service, provider => component());
                    break;
                    case DependencyLifecycle.InstancePerUnitOfWork:
                        services.AddScoped(service, provider => component());
                    break;
                    case DependencyLifecycle.SingleInstance:
                        services.AddSingleton(service, provider => component());
                    break;
                }
            }
        }

        public void Dispose()
        {
            
        }

        public bool HasComponent(Type componentType)
        {
            return services.Any(sd => sd.ServiceType == componentType);
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            services.AddSingleton(lookupType, instance);
            serviceProvider = services.BuildServiceProvider();
        }

        public void Release(object instance)
        {
        }        

        private IEnumerable<Type> GetAllServices<T>()
        {
            var typeOfT = typeof(T);
            var interfaces = typeOfT.GetInterfaces().Where(i => !i.Namespace.StartsWith("System"));
            return interfaces;
        }

        private IEnumerable<Type> GetAllServices(Type typeOfT)
        {
            var interfaces = typeOfT.GetInterfaces().Where(i => !i.Namespace.StartsWith("System"));
            return interfaces;
        }
    }

}