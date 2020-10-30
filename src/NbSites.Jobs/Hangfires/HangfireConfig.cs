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
                //.ConfigMySql(dbConn));
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

        //internal static IGlobalConfiguration ConfigMySql(this IGlobalConfiguration configuration, string dbConn)
        //{
        //    configuration.UseStorage(new MySqlStorage(
        //        dbConn, new MySqlStorageOptions
        //        {
        //            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
        //            QueuePollInterval = TimeSpan.FromSeconds(15),
        //            JobExpirationCheckInterval = TimeSpan.FromHours(1),
        //            CountersAggregateInterval = TimeSpan.FromMinutes(5),
        //            PrepareSchemaIfNecessary = true,
        //            DashboardJobListLimit = 50000,
        //            TransactionTimeout = TimeSpan.FromMinutes(1),
        //            TablesPrefix = "Hangfire"
        //        }));
        //    return configuration;
        //}
    }
}