using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Core.AutoInject
{
    public static class AutoInjectExtensions
    {
        public static IList<string> AutoInject<TInterface>(this IServiceCollection services)
        {
            var logs = new List<string>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("NbSites.")).ToList();
            foreach (var assembly in assemblies)
            {
                logs.Add("AutoInject GetAssembly: " + assembly.FullName);
                var autoInject = services.AutoInject<TInterface>(assembly);
                logs.AddRange(autoInject);
            }
            return logs;
        }

        public static IList<string> AutoInject<TInterface>(this IServiceCollection services, Assembly assembly)
        {
            var logs = new List<string>();
            assembly.GetImplTypesAssignableFrom<TInterface>().ForEach(implType =>
            {
                Console.WriteLine(implType);
                var interfaceType = typeof(TInterface);
                //only add when not exist
                var serviceDescriptor = services.LastOrDefault(descriptor => descriptor.ServiceType == interfaceType && descriptor.ImplementationType == implType);
                if (serviceDescriptor == null)
                {
                    //default IAutoInject set to Transient
                    serviceDescriptor = ServiceDescriptor.Describe(interfaceType, implType, ServiceLifetime.Transient);

                    if (typeof(IAutoInjectAsTransient).IsAssignableFrom(interfaceType) || typeof(IAutoInjectAsTransient).IsAssignableFrom(implType))
                    {
                        serviceDescriptor = ServiceDescriptor.Describe(interfaceType, implType, ServiceLifetime.Transient);
                    }

                    if (typeof(IAutoInjectAsScoped).IsAssignableFrom(interfaceType) || typeof(IAutoInjectAsScoped).IsAssignableFrom(implType))
                    {
                        serviceDescriptor = ServiceDescriptor.Describe(interfaceType, implType, ServiceLifetime.Scoped);
                    }

                    if (typeof(IAutoInjectAsSingleton).IsAssignableFrom(interfaceType) || typeof(IAutoInjectAsSingleton).IsAssignableFrom(implType))
                    {
                        serviceDescriptor = ServiceDescriptor.Describe(interfaceType, implType, ServiceLifetime.Singleton);
                    }

                    logs.Add(" -> AutoInject: " + serviceDescriptor);
                    services.Add(serviceDescriptor);
                }
                else
                {
                    logs.Add(" -> ExistInject: " + serviceDescriptor);
                }
            });
            return logs;
        }
        
        public static List<Type> GetImplTypesAssignableFrom<TInterface>(this Assembly assembly)
        {
            return assembly.GetImplTypesAssignableFrom(typeof(TInterface));
        }

        public static List<Type> GetImplTypesAssignableFrom(this Assembly assembly, Type interfaceType)
        {
            var implTypes = new List<Type>();
            foreach (var implType in assembly.DefinedTypes)
            {
                if (interfaceType.IsAssignableFrom(implType) && interfaceType != implType)
                {
                    if (implType.IsAbstract || implType.IsInterface)
                    {
                        continue;
                    }
                    implTypes.Add(implType);
                }
            }
            return implTypes;
        }
    }
}