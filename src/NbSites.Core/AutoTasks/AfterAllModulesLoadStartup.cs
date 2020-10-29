using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace NbSites.Core.AutoTasks
{
    public class AfterAllModulesLoadStartup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public AfterAllModulesLoadStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override int Order => StartupOrder.Instance.AfterAllModulesLoad;

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            //todo auto bind
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var allTasks = scope.ServiceProvider
                    .GetServices<IAfterAllModulesLoadTask>().ToList();

                var allTasksGroups = allTasks.GroupBy(x => x.Category).ToList();
                foreach (var allTasksGroup in allTasksGroups)
                {
                    var orderedTasks = allTasksGroup.OrderBy(x => x.Order).ToList();
                    foreach (var task in orderedTasks)
                    {
                        task.Run();
                    }
                }
            }
        }
    }
}
