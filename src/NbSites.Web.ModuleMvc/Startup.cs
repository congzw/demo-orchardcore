using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using OrchardCore.Modules;

namespace NbSites.Web.ModuleMvc
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute
            (
                name: "ModuleMvc_Default",
                areaName: "NbSites.Web.ModuleMvc",
                pattern: "ModuleMvc/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
