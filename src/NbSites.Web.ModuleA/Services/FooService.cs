using System;

namespace NbSites.Web.ModuleA.Services
{
    public class FooService
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string GetDesc()
        {
            return "From ModuleA FooService " + Id.ToString("N");
        }
    }
}
