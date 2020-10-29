using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Base.Data;
using NbSites.Core;
using NbSites.Core.EFCore;
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

        public override int Order => StartupOrder.Instance.Base;

        public override void ConfigureServices(IServiceCollection services)
        {
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(this.GetType().Assembly);
            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
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
