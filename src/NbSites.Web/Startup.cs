using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NbSites.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddOrchardCore().WithTenants().AddMvc(); //启动租户
            services.AddOrchardCore().AddMvc(); //不启动租户
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseOrchardCore();
        }
    }
}
