using System;
using Common.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NbSites.Base.AppService.Dev;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.Data;
using NbSites.Core.EFCore;

namespace NbSites.Base
{
    public class Startup : MyStartupBase
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            services.AddOptions<DevSetting>().Bind(Configuration.GetSection("DevSetting"));
            services.AddScoped(sp => sp.GetService<IOptionsSnapshot<DevSetting>>().Value);

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

        //public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        //{
        //    base.Configure(app, routes, serviceProvider);
        //}
    }
}
