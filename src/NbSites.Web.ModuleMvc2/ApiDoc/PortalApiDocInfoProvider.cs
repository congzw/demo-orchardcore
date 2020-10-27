using System.Collections.Generic;
using NbSites.Core.ApiDoc;

namespace NbSites.Web.ModuleMvc2.ApiDoc
{
    public class PortalApiDocInfoProvider : IApiDocInfoProvider
    {
        public IList<ApiDocInfo> GetApiDocInfos()
        {
            var apiDocInfos = new List<ApiDocInfo>();
            apiDocInfos.Add(new ApiDocInfo()
            {
                Hide = false,
                Name = "App-Demo-Foo",
                Title = "应用层模块接口-演示",
                Version = "0.1",
                Description = "应用层模块接口-演示...",
                Endpoint = string.Format("/swagger/{0}/swagger.json", "App-Demo-Foo"),
                XmlFile = "NbSites.Web.ModuleMvc2.xml"
            });
            return apiDocInfos;
        }
    }
}
