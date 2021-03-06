﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OrchardCore.Environment.Shell;

namespace NbSites.Core.Data
{
    public class DbConnConfigConst
    {
        public const string DataProvider = "DataProvider";
    }

    public interface IDbConnConfigHelper
    {
        //string Tenant { get; }
        DbConnConfig GetDbConnConfig(string connName, string dataProvider);
        Task SaveDbConnConfig(DbConnConfig config);
        Task SaveDatabaseCreated(string databaseName, bool created);
    }
    
    public class DbConnConfigHelper : IDbConnConfigHelper
    {

        private readonly ShellSettings _shellSettings;

        private readonly IShellSettingsManager _shellSettingsManager;

        public DbConnConfigHelper(ShellSettings shellSettings, IShellSettingsManager shellSettingsManager, DbConnConfigCache configCache)
        {
            ConfigCache = configCache;
            _shellSettings = shellSettings;
            _shellSettingsManager = shellSettingsManager;
        }

        public string Tenant => _shellSettings.Name;

        public DbConnConfigCache ConfigCache { get; }

        public DbConnConfig GetDbConnConfig(string connName, string dataProvider)
        {
            var config = ConfigCache.Get(Tenant, connName, dataProvider);
            //if (config == null)
            //{
            //    var connectionString = _shellSettings.ShellConfiguration[connName];
            //    if (!string.IsNullOrWhiteSpace(connectionString))
            //    {
            //        config = new DbConnConfig();
            //        config.ConnectionName = connName;
            //        config.ConnectionString = connectionString;
            //        config.DataProvider = dataProvider;
            //        ConfigCache.Save(tenant, config);
            //    }
            //}
            return config;
        }

        public Task SaveDbConnConfig(DbConnConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            ConfigCache.Save(Tenant, config);
            _shellSettings.ShellConfiguration[DbConnConfigConst.DataProvider] = config.DataProvider;
            _shellSettings.ShellConfiguration[config.ConnectionName] = config.ConnectionString;
            return _shellSettingsManager.SaveSettingsAsync(_shellSettings);
        }

        public Task SaveDatabaseCreated(string databaseName, bool created)
        {
            var dbCreated = _shellSettings.ShellConfiguration.GetValue<bool>(databaseName + "Created");
            if (dbCreated)
            {
                return Task.CompletedTask;
            }
            _shellSettings.ShellConfiguration[databaseName + "Created"] = created.ToString();
            return _shellSettingsManager.SaveSettingsAsync(_shellSettings);
        }
    }
    
    public class DbConnConfig
    {
        public string DataProvider { get; set; }
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }

        public static DbConnConfig Create(string connectionName, string dataProvider, string connString)
        {
            var config = new DbConnConfig();
            config.DataProvider = dataProvider;
            config.ConnectionName = connectionName;
            config.ConnectionString = connString;
            return config;
        }
    }

    public class DbConnConfigCache
    {
        public IDictionary<string, DbConnConfig> Connections { get; set; } = new ConcurrentDictionary<string, DbConnConfig>(StringComparer.OrdinalIgnoreCase);

        public void Save(string tenant, DbConnConfig config)
        {
            var key = tenant + ":" + config.DataProvider + ":" + config.ConnectionName;
            Connections[key] = config;
        }

        public DbConnConfig Get(string tenant, string connName, string dataProvider)
        {
            var key = tenant + ":" + dataProvider + ":" + connName;
            var config = Connections.TryGetValue(key, out var connectionConfig) ? connectionConfig : null;
            return config;
        }
    }
}
