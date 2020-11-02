using Common;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace NbSites.Core.AutoTasks
{
    public class BeforeAllModulesLoadStartup : StartupBase
    {
        public override int Order => StartupOrder.Instance.BeforeAllModulesLoad;

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var dbContextHelper = DbContextHelper.Instance;
            dbContextHelper.AddSupportSqlServer();
            dbContextHelper.AddSupportMySql();
            services.AddSingleton(dbContextHelper);
        }

        //public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        //{
        //    base.Configure(app, routes, serviceProvider);
        //}
    }
}
