using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.App.Portal.ApiDoc;
using NbSites.App.Portal.Data;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.ApiDoc;
using NbSites.Core.DataSeed;
using NbSites.Core.EFCore;
using OrchardCore.Modules;

namespace NbSites.App.Portal
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override int Order => StartupOrder.Instance.App;

        public override void ConfigureServices(IServiceCollection services)
        {
            //todo auto find and get
            //ModelAssemblyRegistry.Instance.AddModelConfigAssembly("NbSitesDbContext", this.GetType().Assembly);
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(this.GetType().Assembly);

            services.AddScoped<PortalDbContext>(sp => new PortalDbContext(sp.GetRequiredService<NbSitesDbContext>()));
            services.AddSingleton<IApiDocInfoProvider, PortalApiDocInfoProvider>();
            services.AddScoped<ISeed, PortalSeed>();
        }
        
        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
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
