using System;
using System.Transactions;
using Common.Data;
using Hangfire;
using Hangfire.MySql;
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
            var dataProvider = config["DataProvider"];
            var dbContextHelper = DbContextHelper.Instance;

            var provider = dbContextHelper.AutoFixProvider(dataProvider);
            dbContextHelper.EnsureEmptyDb(dbConn, dataProvider);
            
            services.AddHangfire(gc =>
            {
                gc.ConfigHangfireStorage(dbConn, provider);
            });
            
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

        internal static IGlobalConfiguration ConfigHangfireStorage(this IGlobalConfiguration gc, string dbConn, string theProvider)
        {
            var hangfireConfig = gc
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            if (theProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                hangfireConfig.ConfigSqlServer(dbConn);
                return gc;
            }
            if (theProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            {
                hangfireConfig.ConfigMySql(dbConn);
                return gc;
            }

            throw new NotSupportedException("不支持的数据库类型: " + theProvider);
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

        internal static IGlobalConfiguration ConfigMySql(this IGlobalConfiguration configuration, string dbConn)
        {
            configuration.UseStorage(new MySqlStorage(
                dbConn, new MySqlStorageOptions
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablesPrefix = "Hangfire"
                }));
            return configuration;
        }
    }
}