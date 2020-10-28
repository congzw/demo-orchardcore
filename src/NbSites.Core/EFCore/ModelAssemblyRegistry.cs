using System.Collections.Generic;
using System.Reflection;

namespace NbSites.Core.EFCore
{
    public class ModelAssemblyRegistry
    {
        public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
        public ModelAssemblyRegistry AddModelConfigAssembly(Assembly assembly)
        {
            if (Assemblies.Contains(assembly))
            {
                return this;
            }
            Assemblies.Add(assembly);
            return this;
        }

        public static ModelAssemblyRegistry Instance = new ModelAssemblyRegistry();
    }
}
