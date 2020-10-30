using System.Collections.Generic;
using NbSites.Core.ApiDoc;

namespace NbSites.Jobs.ApiDoc
{
    public class JobApiDocInfoProvider : IApiDocInfoProvider
    {
        public IList<ApiDocInfo> GetApiDocInfos()
        {
            var apiDocInfos = new List<ApiDocInfo>();
            apiDocInfos.Add(new ApiDocInfo()
            {
                Hide = false,
                Name = "Demo-Jobs",
                Title = "演示用的后台任务",
                Version = "0.1",
                Description = "演示用的后台任务",
                Endpoint = string.Format("/swagger/{0}/swagger.json", "Demo-Jobs"),
                XmlFile = "NbSites.Jobs.xml"
            });
            return apiDocInfos;
        }
    }
}
