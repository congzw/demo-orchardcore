using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Jobs.LogIt;
using OrchardCore.Modules;

namespace NbSites.Jobs.Hangfires
{
    //feature?
    //[Feature(JobsConstants.Features.Hangfire)]
    public class HangfireStartup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public HangfireStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override int Order => -1;

        public override void ConfigureServices(IServiceCollection services)
        {
            LogCommandHelper.Instance.LogToFile = true;
            services.AddMyHangfire(Configuration);

            ////this way is not working in orchard module! => use this: UseHangfireServer
            //// Add the processing server as IHostedService
            //services.AddHangfireServer(opt =>
            //{
            //    opt.ServerName = "LightHangfireServer";
            //});
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