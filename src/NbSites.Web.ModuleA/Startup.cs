using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Web.ModuleA.Services;
using StartupBase = OrchardCore.Modules.StartupBase;

namespace NbSites.Web.ModuleA
{
    public class Startup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<FooService>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);

            app.UseEndpoints(endpoints =>
                endpoints.MapGet("/ModuleA/Hello",
                    async context =>
                    {
                        await context.Response.WriteAsync("Hello from ModuleA!");
                    })
            );

            routes.MapAreaControllerRoute
            (
                name: "Root_Home",
                areaName: "NbSites.Web.ModuleA",
                pattern: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapAreaControllerRoute(
                name: "ModuleA_Default",
                areaName: "NbSites.Web.ModuleA",
                pattern: "ModuleA/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
