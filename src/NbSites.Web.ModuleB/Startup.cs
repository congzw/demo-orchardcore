using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Web.ModuleB.Services;
using OrchardCore.Modules;

namespace NbSites.Web.ModuleB
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
                    endpoints.MapGet("/ModuleB/", async context =>
                    {
                        await context.Response.WriteAsync("Hello from ModuleB!");
                    }));
        }
    }
}
