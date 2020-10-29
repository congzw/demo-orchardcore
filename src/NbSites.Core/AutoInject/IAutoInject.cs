using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Core.AutoInject
{
    public interface IAutoInject
    {
    }

    //public interface IAutoInjectAsScope
    //{
    //}

    public static class AssemblyExtensions
    {
        public static void AutoInject<TInterface>(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime, bool overrideExist = false)
        {
            assembly.GetImplTypesAssignableFrom<TInterface>().ForEach(implType =>
            {
                var interfaceType = typeof(TInterface);
                //if (overrideExist)
                //{
                //    //todo: check exist and ignore
                //    var exist = IsExist(services, interfaceType, implType)
                //}
                var serviceDescriptor = ServiceDescriptor.Describe(interfaceType, implType, serviceLifetime);
                services.Add(serviceDescriptor);
            });
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
