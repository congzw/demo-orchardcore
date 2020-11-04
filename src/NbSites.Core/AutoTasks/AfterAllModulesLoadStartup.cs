using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NbSites.Core.AutoInject;

namespace NbSites.Core.AutoTasks
{
    public class AfterAllModulesLoadStartup : MyStartupBase
    {
        private readonly ILogger<AfterAllModulesLoadStartup> _logger;
        public IConfiguration Configuration { get; }

        public AfterAllModulesLoadStartup(IConfiguration configuration, ILogger<AfterAllModulesLoadStartup> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var autoInjectRegistry = AutoInjectRegistry.Instance;
            services.AddSingleton(autoInjectRegistry);
            services.AutoInject(autoInjectRegistry);
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var allTasks = scope.ServiceProvider
                    .GetServices<IAfterAllModulesLoadTask>().ToList();

                var allTasksGroups = allTasks.GroupBy(x => x.Category).ToList();
                
                //确保数据生成
                var dataSeedGroup = allTasksGroups.FirstOrDefault(x => x.Key == "DataSeed");
                if (dataSeedGroup != null)
                {
                    var dataSeeds = dataSeedGroup.OrderBy(x => x.Order).ToList();
                    foreach (var dataSeed in dataSeeds)
                    {
                        dataSeed.Run();
                    }
                }

                //其他任务
                var otherGroups = allTasksGroups.Where(x => x.Key != "DataSeed").ToList();
                foreach (var otherGroup in otherGroups)
                {
                    var orderedTasks = otherGroup.OrderBy(x => x.Order).ToList();
                    foreach (var task in orderedTasks)
                    {
                        task.Run();
                    }
                }
            }
        }
    }
}
