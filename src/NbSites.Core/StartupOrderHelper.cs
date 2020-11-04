using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OrchardCore.Modules;

namespace NbSites.Core
{
    public interface IStartupOrderHelper
    {
        int? TryGetOrder(IStartup startup, int? defaultValue);
        int? TryGetConfigureOrder(IStartup startup, int? defaultValue);

        IList<string> ConfigureServicesInvokeLogs { get; set; }
        IList<string> ConfigureInvokeLogs { get; set; }
    }

    public class StartupOrderHelper : IStartupOrderHelper
    {
        public StartupOrderHelper()
        {
            Orders.Add("NbSites.Core.AutoTasks.BeforeAllModulesLoadStartup", StartupOrder.Instance.BeforeAllModulesLoad);
            Orders.Add("NbSites.ApiDoc.Startup", StartupOrder.Instance.AfterAllModulesLoad + 1);
            Orders.Add("NbSites.Core.AutoTasks.AfterAllModulesLoadStartup", StartupOrder.Instance.AfterAllModulesLoad);
            Orders.Add("NbSites.Base.Startup", StartupOrder.Instance.Base);
            Orders.Add("NbSites.Jobs.Startup", StartupOrder.Instance.BaseMin - 100);
            Orders.Add("NbSites.Core.Startup", StartupOrder.Instance.Core);
            //extension: read from config set Orders before use
        }

        public IDictionary<string, int?> Orders { get; set; } = new ConcurrentDictionary<string, int?>(StringComparer.OrdinalIgnoreCase);

        public int? TryGetOrder(IStartup startup, int? defaultValue)
        {
            return Orders.TryGetValue(startup.GetType().FullName ?? throw new InvalidOperationException(), out var theOrder) ? theOrder : defaultValue;
        }

        public IDictionary<string, int?> ConfigureOrders { get; set; } = new ConcurrentDictionary<string, int?>(StringComparer.OrdinalIgnoreCase);
        public int? TryGetConfigureOrder(IStartup startup, int? defaultValue)
        {
            if (ConfigureOrders.TryGetValue(startup.GetType().FullName ?? throw new InvalidOperationException(), out var configureOrder))
            {
                return configureOrder;
            }

            if (Orders.TryGetValue(startup.GetType().FullName ?? throw new InvalidOperationException(), out var order))
            {
                return order;
            }

            return defaultValue;
        }

        public IList<string> ConfigureServicesInvokeLogs { get; set; } = new List<string>();
        public IList<string> ConfigureInvokeLogs { get; set; } = new List<string>();


        public static Func<IStartupOrderHelper> Instance = () => Lazy.Value;

        private static readonly Lazy<StartupOrderHelper> Lazy = new Lazy<StartupOrderHelper>(() => new StartupOrderHelper());
    }
    
    public static class StartupExtensions
    {
        public static int? TryGetOrder(this IStartup startup, int? defaultValue)
        {
            return StartupOrderHelper.Instance().TryGetOrder(startup, defaultValue);
        }

        public static int TryGetOrderForApp(this IStartup startup, int? defaultValue = null)
        {
            var theOrder = TryGetOrder(startup, defaultValue);
            return theOrder ?? StartupOrder.Instance.App;
        }

        public static int TryGetOrderForBase(this IStartup startup, int? defaultValue = null)
        {
            var theOrder = TryGetOrder(startup, defaultValue);
            return theOrder ?? StartupOrder.Instance.Base;
        }

        public static void LogConfigServices(this IStartup startup)
        {
            StartupOrderHelper.Instance().ConfigureServicesInvokeLogs.Add(startup.GetType().FullName + ":" + startup.Order);
        }


        public static int? TryGetConfigureOrder(this IStartup startup, int? defaultValue)
        {
            return StartupOrderHelper.Instance().TryGetConfigureOrder(startup, defaultValue);
        }

        public static int TryGetConfigureOrderForApp(this IStartup startup, int? defaultValue = null)
        {
            var theOrder = TryGetConfigureOrder(startup, defaultValue);
            return theOrder ?? StartupOrder.Instance.App;
        }

        public static int TryGetConfigureOrderForBase(this IStartup startup, int? defaultValue = null)
        {
            var theOrder = TryGetConfigureOrder(startup, defaultValue);
            return theOrder ?? StartupOrder.Instance.Base;
        }
        
        public static void LogConfigure(this IStartup startup)
        {
            StartupOrderHelper.Instance().ConfigureInvokeLogs.Add(startup.GetType().FullName + ":" + startup.ConfigureOrder);
        }
    }
}
