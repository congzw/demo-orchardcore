using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.App.Portal.Data;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.EFCore;

namespace NbSites.App.Portal
{
    public class Startup : MyAppStartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(this.GetType().Assembly);
            services.AddScoped<PortalDbContext>(sp => new PortalDbContext(sp.GetRequiredService<BaseDbContext>()));
        }
        
        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(builder, routes, serviceProvider);

            routes.MapAreaControllerRoute
            (
                name: "Portal_Root",
                areaName: "NbSites.App.Portal",
                pattern: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapAreaControllerRoute
            (
                name: "Portal_Default",
                areaName: "NbSites.App.Portal",
                pattern: "Portal/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
