using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public class DbContextSetupAction
    {
        public string DataProvider { get; set; }
        public Action<DbContextOptionsBuilder, string> SetupAction { get; set; }

        public static DbContextSetupAction Create(string dataProvider, Action<DbContextOptionsBuilder, string> setupAction)
        {
            return new DbContextSetupAction() { DataProvider = dataProvider, SetupAction = setupAction };
        }
    }

    public class DbContextHelper
    {
        public IDictionary<string, DbContextSetupAction> SetupActions { get; set; } = new ConcurrentDictionary<string, DbContextSetupAction>(StringComparer.OrdinalIgnoreCase);
        
        public DbContextHelper AddSupport(DbContextSetupAction setupAction)
        {
            setupAction.DataProvider = AutoFixProvider(setupAction.DataProvider);
            SetupActions[setupAction.DataProvider] = setupAction;
            return this;
        }

        public DbContextOptionsBuilder SetupDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string provider, string dbConn)
        {
            if (SetupActions == null || SetupActions.Count == 0)
            {
                throw new InvalidOperationException("DbContextHelper需要至少初始化AddSupport才能使用");
            }

            var dataProvider = AutoFixProvider(provider);
            if (SetupActions.TryGetValue(dataProvider, out var theAction))
            {
                theAction.SetupAction.Invoke(optionsBuilder, dbConn);
                return optionsBuilder;
            }
            throw new NotSupportedException("不支持的数据库类型: " + provider);
        }
        
        //public DbContextOptionsBuilder SetupDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder, string provider, string dbConn)
        //{
        //    var fixProvider = AutoFixProvider(provider);

        //    if (fixProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        //    {
        //        optionsBuilder.UseSqlServer(dbConn);
        //        return optionsBuilder;
        //    }

        //    if (fixProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
        //    {
        //        //8.0.19.0 => 8.0.22.0
        //        //System.TypeLoadException:“Method 'get_Info' in type 'MySql.Data.EntityFrameworkCore.Infraestructure.MySQLOptionsExtension'
        //        //from assembly 'MySql.Data.EntityFrameworkCore, Version=8.0.19.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d' does not have an implementation.”
        //        optionsBuilder.UseMySQL(dbConn);
        //        return optionsBuilder;
        //    }
        //    throw new NotSupportedException("不支持的数据库类型: " + provider);
        //}

        public void EnsureEmptyDb(string dbConn, string dataProvider)
        {
            var provider = AutoFixProvider(dataProvider);
            using (var dbContext = new AutoCreateEmptyDbContext(dbConn, provider))
            {
                if (!dbContext.Database.CanConnect())
                {
                    dbContext.Database.EnsureCreated();
                }
            }
        }
        
        public string AutoFixProvider(string provider)
        {
            return !string.IsNullOrWhiteSpace(provider) ? provider : "SqlServer";
        }

        public static DbContextHelper Instance = new DbContextHelper();
    }

    public static class DbContextHelperExtensions
    {
        public static string GetDataProviderFromConfiguration(this DbContextHelper helper, IConfiguration configuration)
        {
            //template for non tenant
            var dataProvider = configuration["DataProvider"];
            var fixProvider = helper.AutoFixProvider(dataProvider);
            return fixProvider;
        }

        public static DbContextHelper AddSupportSqlServer(this DbContextHelper helper, Action<DbContextOptionsBuilder, string> setupAction = null)
        {
            setupAction ??= (builder, connString) => { builder.UseSqlServer(connString); };
            helper.AddSupport(DbContextSetupAction.Create("SqlServer", setupAction));
            return helper;
        }

        public static DbContextHelper AddSupportMySql(this DbContextHelper helper, Action<DbContextOptionsBuilder, string> setupAction = null)
        {
            setupAction ??= (builder, connString) => { builder.UseMySQL(connString); };
            helper.AddSupport(DbContextSetupAction.Create("MySql", setupAction));
            return helper;
        }
    }

    internal class AutoCreateEmptyDbContext : DbContext
    {
        public string Provider { get; set; }

        public string DbConn { get; }

        internal AutoCreateEmptyDbContext(string dbConn, string provider)
        {
            Provider = provider;
            DbConn = dbConn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbContextHelper.Instance.SetupDbContextOptionsBuilder(optionsBuilder, Provider, DbConn);
        }
    }
}
