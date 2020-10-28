using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Core.DataSeed;
using OrchardCore.Modules;

namespace NbSites.Core
{
    public class LastStartup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public LastStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override int Order => StartupOrder.Instance.AfterAllModulesLoad;

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            //todo auto bind
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var seeds = scope.ServiceProvider.GetServices<ISeed>();
                foreach (var seed in seeds)
                {
                    seed.Init();
                }
            }
        }
    }
}
