using System;

namespace NbSites.Web.ModuleB.Services
{
    public class FooService
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string GetDesc()
        {
            return "From ModuleB FooService " + Id.ToString("N");  
        }
    }
}
