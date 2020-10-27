using System.Collections.Generic;
using System.Linq;

namespace NbSites.Core.ApiDoc
{
    public class ApiDocInfoRegistry
    {
        private readonly IList<IApiDocInfoProvider> _providers;

        public ApiDocInfoRegistry(IEnumerable<IApiDocInfoProvider> providers)
        {
            _providers = providers.ToList();
            var apiDocInfos = providers.SelectMany(x => x.GetApiDocInfos()).ToList();
            ApiDocInfos = apiDocInfos;
        }

        public IList<ApiDocInfo> ApiDocInfos { get; set; } 
    }
}