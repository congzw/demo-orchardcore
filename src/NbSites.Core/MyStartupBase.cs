using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace NbSites.Core
{
    public abstract class MyStartupBase : StartupBase
    {
        public override int Order => this.TryGetOrderForBase();

        public override int ConfigureOrder => this.TryGetConfigureOrderForBase();

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            this.LogConfigServices();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);
            this.LogConfigure();
        }
    }

    public abstract class MyAppStartupBase : MyStartupBase
    {
        public override int Order => this.TryGetOrderForApp();
        public override int ConfigureOrder => this.TryGetConfigureOrderForApp();
    }
}