using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NbSites.ApiDoc.Boots;
using NbSites.Core;
using NbSites.Core.ApiDoc;

namespace NbSites.ApiDoc
{
    public class Startup : MyStartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<ApiDocInfoRegistry>();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                ApiDocInfoRegistry apiDocInfoRegistry = scope.ServiceProvider.GetRequiredService<ApiDocInfoRegistry>();
                //AddApiDoc在注册阶段要求Build，以获取所有的文档提供者信息
                services.AddApiDoc(apiDocInfoRegistry);
            }
        }
        
        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(builder, routes, serviceProvider);

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
