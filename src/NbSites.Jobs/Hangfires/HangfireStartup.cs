using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Core;
using NbSites.Jobs.LogIt;

namespace NbSites.Jobs.Hangfires
{
    //feature?
    //[Feature(JobsConstants.Features.Hangfire)]
    public class HangfireStartup : MyStartupBase
    {
        public IConfiguration Configuration { get; }

        public HangfireStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<DelayCallCommand>();
            LogCommandHelper.Instance.LogToFile = true;
            services.AddMyHangfire(Configuration);
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            builder.UseHangfireServer(new BackgroundJobServerOptions()
            {
                ServerName = "LightHangfireServer"
            });

            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    DashboardTitle = "后台任务服务器",
                    Authorization = new[] { new MyDashboardAuthorizationFilter() }
                });
            });
        }
    }

    public static class JobsConstants
    {
        public static class Features
        {
            public const string Hangfire = "NbSites.Jobs.Hangfire";
        }
    }
}