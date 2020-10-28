using System.Collections.Generic;
using System.Linq;

namespace NbSites.Core.ApiDoc
{
    public class ApiDocInfoRegistry
    {
        public ApiDocInfoRegistry(IEnumerable<IApiDocInfoProvider> providers)
        {
            var all = providers.ToList();
            var apiDocInfos = all.SelectMany(x => x.GetApiDocInfos()).ToList();
            ApiDocInfos = apiDocInfos;
        }

        public IList<ApiDocInfo> ApiDocInfos { get; set; }
    }
}