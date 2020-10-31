using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NbSites.Jobs.Hangfires
{
    public static class HangfireConfig
    {
        public static void AddMyHangfire(this IServiceCollection services, IConfiguration config)
        {
            //ensure the hangfireDb exist
            var dbConn = config.GetConnectionString("HangfireConnection");
            using (var hangfireDbContext = new HangfireDbContext(dbConn))
            {
                hangfireDbContext.Database.EnsureCreated();
            }

            services.AddHangfire(gc => gc
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .ConfigSqlServer(dbConn));

            ////多租户有些问题，分发和处理不同源，会不会是问题，有无必要考虑多租户？
            //services.AddHangfire((sp, gc) =>
            //{
            //    var myDatabaseHelper = sp.GetRequiredService<MyDatabaseHelper>();
            //    var databaseConfig = myDatabaseHelper.GetMyTenantConnectionConfig("HangfireConnection");
            //    if (databaseConfig == null)
            //    {
            //        var tenant = myDatabaseHelper.Tenant;
            //        databaseConfig = MyDatabaseConfig.Create(
            //            connectionName: "HangfireConnection",
            //            dataProvider: "SqlServer",
            //            tenant: tenant,
            //            connString: $"Server=(localdb)\\MSSQLLocalDB; Database=DemoHangfireDb_{tenant}; Trusted_Connection=True; MultipleActiveResultSets=true");
            //        myDatabaseHelper.InitConfig(databaseConfig).Wait();
            //        //创建数据库
            //        myDatabaseHelper.EnsureCreateDatabase(new HangfireDbContext(databaseConfig.ConnectionString), "DemoHangfireDb").Wait();
            //    }

            //    gc.ConfigSqlServer(databaseConfig.ConnectionString);
            //});
        }

        internal static IGlobalConfiguration ConfigSqlServer(this IGlobalConfiguration configuration, string dbConn)
        {
            configuration.UseSqlServerStorage(dbConn, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            });

            return configuration;
        }
    }
}