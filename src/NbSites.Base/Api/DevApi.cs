using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NbSites.Base.Data;
using NbSites.Core.AutoTasks;

namespace NbSites.Base.Api
{
    [ApiExplorerSettings(GroupName = "Base-Dev")]
    [Route("~/Api/Base/Dev/[action]")]
    [ApiController]
    public class DevApi : ControllerBase
    {
        [HttpGet]
        public string GetInfo()
        {
            return this.GetType().FullName;
        }

        [HttpGet]
        public string ResetDb([FromServices] BaseDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return "Reset OK";
        }

        [HttpGet]
        public string DataSeed([FromServices] IEnumerable<IAfterAllModulesLoadTask> afterAllModulesLoadTasks)
        {
            var dataSeedTasks  = afterAllModulesLoadTasks.Where(x => x.Category == "DataSeed").OrderBy(x => x.Order).ToList();
            foreach (var dataSeedTask in dataSeedTasks)
            {
                dataSeedTask.Run();
            }
            return string.Join(',', dataSeedTasks.Select(x => x.GetType().Name));
        }
    }
}
