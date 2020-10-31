using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.Data;
using NbSites.Core.EFCore;
using OrchardCore.Modules;

namespace NbSites.Base
{
    public class Startup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override int Order => StartupOrder.Instance.Base;

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMyDatabaseHelper, MyDatabaseHelper>();
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(this.GetType().Assembly);

            services.AddDbContext<BaseDbContext>(async (sp, options) =>
            {
                var myDatabaseHelper = sp.GetRequiredService<IMyDatabaseHelper>();
                var config = myDatabaseHelper.GetMyTenantConnectionConfig("DefaultConnection");
                if (config == null)
                {
                    var tenant = myDatabaseHelper.Tenant;
                    config = MyDatabaseConfig.Create(
                        connectionName:"DefaultConnection", 
                        dataProvider:"SqlServer", 
                        tenant:tenant, 
                        connString: $"Server=(localdb)\\MSSQLLocalDB; Database=DemoDb_{tenant}; Trusted_Connection=True; MultipleActiveResultSets=true");
                    await myDatabaseHelper.InitConfig(config);
                }
                options.UseSqlServer(config.ConnectionString);
            });
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);

            //app.UseEndpoints(endpoints =>
            //        endpoints.MapGet("/Base/Hello", async context =>
            //        {
            //            await context.Response.WriteAsync("Hello from Module Base!");
            //        }));

            UseDatabase(app);
        }

        private static void UseDatabase(IApplicationBuilder applicationBuilder)
        {
            var serviceScopeFactory = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
                var context = serviceScope.ServiceProvider.GetService<BaseDbContext>();
                if (context == null)
                {
                    logger.LogWarning("无法正常初始化数据库,可能是租户首次加载");
                    return;
                }

                var myDatabaseHelper = serviceScope.ServiceProvider.GetService<IMyDatabaseHelper>();
                myDatabaseHelper.EnsureCreateDatabase(context);
            }
        }
    }
}
