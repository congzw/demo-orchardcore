using System;
using Common;
using Common.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.Context;
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
            services.AddTransient<TenantContext>();
            services.AddSingleton<DbConnConfigCache>();
            services.AddTransient<DbConnConfigHelper>();
            services.AddTransient<IDbConnConfigHelper, DbConnConfigHelper>();


            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(this.GetType().Assembly);

            services.AddDbContext<BaseDbContext>(async (sp, optionsBuilder) =>
            {
                var dbContextHelper = sp.GetRequiredService<DbContextHelper>();
                var dataProvider = dbContextHelper.GetDataProviderFromConfiguration(Configuration);
                var fixProvider = dbContextHelper.AutoFixProvider(dataProvider);
                
                var connName = "DefaultConnection";
                var databaseName = "DemoDb";
                //template for non tenant
                var dbConn = Configuration.GetConnectionString(connName);
                
                var myDatabaseHelper = sp.GetRequiredService<DbConnConfigHelper>();
                var config = myDatabaseHelper.GetDbConnConfig(connName, dataProvider);
                if (config == null)
                {
                    //create tenant Conn from "non tenant" template Conn
                    //eg => replace DemoDb with DemoDb_{tenant}
                    var tenant = myDatabaseHelper.Tenant;
                    var dbConnTenant = dbConn.Replace(databaseName, $"{databaseName}_{tenant}", StringComparison.OrdinalIgnoreCase);
                    config = DbConnConfig.Create(
                        connectionName: connName,
                        dataProvider: fixProvider,
                        connString: dbConnTenant);

                    dbContextHelper.SetupDbContextOptionsBuilder(optionsBuilder, fixProvider, dbConnTenant);
                    await using (var baseDbContext = new BaseDbContext(optionsBuilder.Options))
                    {
                        await myDatabaseHelper.SaveDbConnConfig(config);
                        var canConnect = await baseDbContext.Database.CanConnectAsync();
                        if (!canConnect)
                        {
                            await baseDbContext.Database.EnsureCreatedAsync();
                        }
                        await myDatabaseHelper.SaveDatabaseCreated(databaseName, true);
                    }
                }

                dbContextHelper.SetupDbContextOptionsBuilder(optionsBuilder, config.DataProvider, config.ConnectionString);
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

            //UseDatabase(app);
        }

        //private static void UseDatabase(IApplicationBuilder applicationBuilder)
        //{
        //    var serviceScopeFactory = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>();
        //    using (var serviceScope = serviceScopeFactory.CreateScope())
        //    {
        //        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<BaseDbContext>>();
        //        var context = serviceScope.ServiceProvider.GetService<AutoCreateDbContext>();
        //        if (context == null)
        //        {
        //            logger.LogWarning("无法正常初始化数据库,可能是租户首次加载");
        //            return;
        //        }

        //        var myDatabaseHelper = serviceScope.ServiceProvider.GetService<MyDatabaseHelper>();
        //        myDatabaseHelper.EnsureCreateDatabase(context, "DemoDb");
        //    }
        //}
    }
}
