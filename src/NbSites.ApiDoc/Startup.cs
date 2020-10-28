using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NbSites.ApiDoc.Boots;
using NbSites.Core;
using NbSites.Core.ApiDoc;
using OrchardCore.Modules;

namespace NbSites.ApiDoc
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public override int Order => StartupOrder.Instance.AfterAllModulesLoad;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ApiDocInfoRegistry>();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                ApiDocInfoRegistry apiDocInfoRegistry = scope.ServiceProvider.GetRequiredService<ApiDocInfoRegistry>();
                services.AddApiDoc(apiDocInfoRegistry);
            }
        }
        
        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var apiDocInfoRegistry = serviceProvider.GetRequiredService<ApiDocInfoRegistry>();
            builder.UseApiDoc(apiDocInfoRegistry);

            routes.MapAreaControllerRoute
            (
                name: "ApiDoc_Default",
                areaName: "NbSites.ApiDoc",
                pattern: "ApiDoc/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
