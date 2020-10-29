using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Core.AutoInject
{
    public static class AutoInjectRegistryExtensions
    {
        public static void AutoInject(this IServiceCollection services, AutoInjectRegistry autoInjectRegistry, Func<IList<Assembly>> getAssemblies = null)
        {
            getAssemblies ??= () => AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("NbSites.")).ToList();

            var assemblies = getAssemblies();
            autoInjectRegistry.AutoRegister(services, assemblies);
        }
    }
}