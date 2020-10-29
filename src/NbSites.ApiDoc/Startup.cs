using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NbSites.ApiDoc.Boots;
using NbSites.Core;
using NbSites.Core.ApiDoc;
using NbSites.Core.AutoInject;
using OrchardCore.Modules;

namespace NbSites.ApiDoc
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public override int Order => StartupOrder.Instance.AfterAllModulesLoad;
        
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            var apiDocInjects = services.AutoInject<IApiDocInfoProvider>();
            foreach (var apiDocInject in apiDocInjects)
            {
                _logger.LogInformation(apiDocInject);
            }

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
