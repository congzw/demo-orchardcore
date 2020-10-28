using System.Collections.Generic;
using NbSites.Core.ApiDoc;

namespace NbSites.App.Portal.ApiDoc
{
    public class PortalApiDocInfoProvider : IApiDocInfoProvider
    {
        public IList<ApiDocInfo> GetApiDocInfos()
        {
            var apiDocInfos = new List<ApiDocInfo>();
            apiDocInfos.Add(new ApiDocInfo()
            {
                Hide = false,
                Name = "App-Portal-Blog",
                Title = "应用层模块接口-演示",
                Version = "0.1",
                Description = "应用层模块接口-演示...",
                Endpoint = string.Format("/swagger/{0}/swagger.json", "App-Portal-Blog"),
                XmlFile = "NbSites.App.Portal.xml"
            });
            return apiDocInfos;
        }
    }
}
