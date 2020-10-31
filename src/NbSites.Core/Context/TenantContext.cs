using OrchardCore.Environment.Shell;

namespace NbSites.Core.Context
{
    public class TenantContext
    {
        private readonly ShellSettings _settings;

        public TenantContext(ShellSettings settings)
        {
            _settings = settings;
        }

        public string Tenant => _settings.Name;
    }
}
