using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Base.AppService.Dev;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.AutoInject;
using NbSites.Core.AutoTasks;
using NbSites.VersionInfos;
using OrchardCore.Environment.Shell;

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
        public string ResetDb([FromServices] BaseDbContext dbContext, [FromServices] DevSetting devSetting)
        {
            if (!devSetting.AllowedResetDatabase)
            {
                return "NotAllowedResetDatable!";
            }

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return "Reset OK";
        }

        [HttpGet]
        public string DataSeed([FromServices] IEnumerable<IAfterAllModulesLoadTask> afterAllModulesLoadTasks)
        {
            var dataSeedTasks = afterAllModulesLoadTasks.Where(x => x.Category == "DataSeed").OrderBy(x => x.Order).ToList();
            foreach (var dataSeedTask in dataSeedTasks)
            {
                dataSeedTask.Run();
            }
            return string.Join(',', dataSeedTasks.Select(x => x.GetType().Name));
        }

        [HttpGet]
        public IList<ClassTypeInfo> GetAutoInjects([FromServices] AutoInjectRegistry autoInjectRegistry)
        {
            var autoRegisterServiceCache = autoInjectRegistry.Cache;
            return autoRegisterServiceCache.ToClassTypeInfos();
        }

        [HttpGet]
        public IStartupOrderHelper GetStartupOrderHelper()
        {
            return StartupOrderHelper.Instance();
        }

        [HttpGet]
        public VersionInfo GetVersionInfo([FromServices] VersionInfo versionInfo)
        {
            return versionInfo;
        }

        [HttpGet]
        public ShellSettings GetShellSettings([FromServices] IServiceProvider sp)
        {
            var shellSettings = sp.GetService<ShellSettings>();
            return shellSettings;
        }

        [HttpGet]
        public string GetDbContext([FromServices] BaseDbContext dbContext)
        {
            return dbContext.Database.GetDbConnection().ConnectionString;
        }
    }
}
