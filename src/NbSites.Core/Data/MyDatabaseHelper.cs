using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;

namespace NbSites.Core.Data
{
    public class MyDatabaseConfig
    {
        public string Tenant { get; set; }
        public string DataProvider { get; set; }
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }

        public static MyDatabaseConfig Create(string connectionName, string dataProvider, string tenant, string connString)
        {
            var config = new MyDatabaseConfig();
            config.Tenant = tenant;
            config.ConnectionString = connString;
            config.DataProvider = connectionName;
            config.ConnectionName = dataProvider;
            return config;
        }
    }

    public class MyDatabaseConst
    {
        public const string DataProvider = "DatabaseProvider";
        public const string DbCreated = "DbCreated";
    }

    public interface IMyDatabaseHelper
    {
        string Tenant { get; }
        MyDatabaseConfig GetMyTenantConnectionConfig(string connName);
        Task InitConfig(MyDatabaseConfig config);
        Task EnsureCreateDatabase(DbContext context);
    }

    public class MyDatabaseHelper : IMyDatabaseHelper
    {
        private readonly ShellSettings _shellSettings;
        private readonly IShellSettingsManager _shellSettingsManager;
        private readonly ILogger<MyDatabaseHelper> _logger;

        public MyDatabaseHelper(ShellSettings shellSettings, IShellSettingsManager shellSettingsManager, ILogger<MyDatabaseHelper> logger)
        {
            _shellSettings = shellSettings;
            _shellSettingsManager = shellSettingsManager;
            _logger = logger;
        }

        public IDictionary<string, MyDatabaseConfig> Connections { get; set; } = new ConcurrentDictionary<string, MyDatabaseConfig>(StringComparer.OrdinalIgnoreCase);
        
        public string Tenant => _shellSettings.Name;
        
        public MyDatabaseConfig GetMyTenantConnectionConfig(string connName)
        {
            var tenant = _shellSettings.Name;
            var key = tenant + ":" + connName;
            var config = Connections.TryGetValue(key, out var connectionConfig) ? connectionConfig : null;

            if (config == null)
            {
                //try read from settings
                var connectionString = _shellSettings.ShellConfiguration[connName];
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    config = new MyDatabaseConfig();
                    config.Tenant = tenant;
                    config.ConnectionName = connName;
                    config.ConnectionString = connectionString;

                    Connections.Add(key, config);
                }
            }
            return config;
        }
        
        public Task InitConfig(MyDatabaseConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _shellSettings.ShellConfiguration[MyDatabaseConst.DataProvider] = config.DataProvider;
            _shellSettings.ShellConfiguration[config.ConnectionName] = config.ConnectionString;
            return _shellSettingsManager.SaveSettingsAsync(_shellSettings);
        }
        
        public Task EnsureCreateDatabase(DbContext context)
        {
            var dbCreated = _shellSettings.ShellConfiguration.GetValue<bool>(MyDatabaseConst.DbCreated);
            if (!dbCreated)
            {
                return Task.CompletedTask;
            }

            context.Database.EnsureCreated();
            _shellSettings.ShellConfiguration[MyDatabaseConst.DbCreated] = true.ToString();
            return _shellSettingsManager.SaveSettingsAsync(_shellSettings);
        }
    }

    //public static class MyDbContextExtensions
    //{
    //    public static IServiceCollection AddMyDbContext<TContext>([NotNull] this IServiceCollection services,
    //        string dataProvider,
    //        string databaseName,
    //        string connName,
    //        [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder, MyDatabaseConfig> optionsAction,
    //        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
    //        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    //        where TContext : DbContext
    //    {

    //        services.AddDbContext<TContext>(async (sp, options) =>
    //        {
    //            var myDatabaseHelper = sp.GetRequiredService<IMyDatabaseHelper>();
    //            var tenant = myDatabaseHelper.Tenant;

    //            var config = myDatabaseHelper.GetMyTenantConnectionConfig("DefaultConnection");
    //            if (config == null)
    //            {
    //                var connString = $"Server=(localdb)\\MSSQLLocalDB; Database={databaseName}_{tenant}; Trusted_Connection=True; MultipleActiveResultSets=true";
    //                config = new MyDatabaseConfig();
    //                config.DataProvider = dataProvider;
    //                config.ConnectionName = connName;
    //                config.Tenant = tenant;
    //                config.ConnectionString = connString;
    //                await myDatabaseHelper.InitConfig(config);
    //            }

    //            optionsAction?.Invoke(sp, options, config);
    //        });

    //        return services;
    //    }
    //}
}
