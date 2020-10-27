using System.Collections.Generic;
using NbSites.Core.ApiDoc;

namespace NbSites.Base.ApiDoc
{
    public class BaseApiDocInfoProvider : IApiDocInfoProvider
    {
        public IList<ApiDocInfo> GetApiDocInfos()
        {
            var apiDocInfos = new List<ApiDocInfo>();
            apiDocInfos.Add(new ApiDocInfo()
            {
                Name = "All-ReadMe",
                Title = "全局接口约定和规范说明",
                Version = "0.1",
                Description = "<p>此文档中列出的接口约定，适用于<em>[全局]</em>。</p>" +
                              "<p>请求状态码采用简化的子集</p>" +
                              "<ul><li>200: （正常应答）服务端处理了请求</li><li>400: （无效请求） 服务端视为无效的请求</li><li>401: （未认证）服务端要求身份验证</li><li>403: （禁止） 服务端视拒绝处理，例如权限不足</li><li>500: （异常）服务端处理出错</li></ul>" +
                              "<p>在其他接口中除非特殊说明，否则同样适用，但不再进行重复说明。 </p>",
                Endpoint = string.Format("/swagger/{0}/swagger.json", "All-ReadMe"),
                XmlFile = "NbSites.Base.xml"
            });


            apiDocInfos.Add(new ApiDocInfo()
            {
                Name = "Base-Products",
                Title = "基础服务接口-产品",
                Version = "0.1",
                Description = "基础服务接口-产品...",
                Endpoint = string.Format("/swagger/{0}/swagger.json", "Base-Products"),
                XmlFile = "NbSites.Base.xml"
            });


            return apiDocInfos;
        }
    }
}
