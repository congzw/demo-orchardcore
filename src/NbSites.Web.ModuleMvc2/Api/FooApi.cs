using Microsoft.AspNetCore.Mvc;

namespace NbSites.Web.ModuleMvc2.Api
{
    [ApiExplorerSettings(GroupName = "App-Demo-Foo")]
    [Route("~/Api/App/Demo/Foo/[action]")]
    [ApiController]
    public class FooApi : ControllerBase
    {
        [HttpGet]
        public string GetInfo()
        {
            return this.GetType().FullName;
        }
    }
}
