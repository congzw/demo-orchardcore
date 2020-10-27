using System.Collections.Generic;

namespace NbSites.Core.ApiDoc
{
    public interface IApiDocInfoProvider
    {
        IList<ApiDocInfo> GetApiDocInfos();
    }
}