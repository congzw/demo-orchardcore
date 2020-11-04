using Common.Data;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Core.AutoTasks
{
    public class BeforeAllModulesLoadStartup : MyStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var startupOrderHelper = StartupOrderHelper.Instance();
            services.AddSingleton<IStartupOrderHelper>(sp => startupOrderHelper);
            services.AddSingleton<StartupOrderHelper>(sp => startupOrderHelper as StartupOrderHelper);

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
