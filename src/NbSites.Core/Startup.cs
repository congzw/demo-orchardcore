﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NbSites.Core.Context;
using NbSites.Core.Data;

namespace NbSites.Core
{
    public class Startup : MyStartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<TenantContext>();
            services.AddSingleton<DbConnConfigCache>();
            services.AddTransient<DbConnConfigHelper>();
            services.AddTransient<IDbConnConfigHelper, DbConnConfigHelper>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(app, routes, serviceProvider);
        }
    }
}
