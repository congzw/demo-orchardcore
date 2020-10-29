using System.Collections.Generic;
using NbSites.Core.AutoInject;

namespace NbSites.Core.ApiDoc
{
    public interface IApiDocInfoProvider : IAutoInjectAsSingleton
    {
        IList<ApiDocInfo> GetApiDocInfos();
    }
}