using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Base.ApiDoc;
using NbSites.Core.ApiDoc;
using OrchardCore.Modules;

namespace NbSites.Base
{
    public class Startup : StartupBase
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override int Order => 998;

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApiDocInfoProvider, BaseApiDocInfoProvider>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);

            //app.UseEndpoints(endpoints =>
            //        endpoints.MapGet("/Base/Hello", async context =>
            //        {
            //            await context.Response.WriteAsync("Hello from Module Base!");
            //        }));

            //routes.MapAreaControllerRoute(
            //    name: "Base_Default",
            //    areaName: "NbSites.Base",
            //    pattern: "Base/{controller}/{action}",
            //    defaults: new { controller = "Home", action = "Index" }
            //);
        }
    }
}
