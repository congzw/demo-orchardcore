using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NbSites.Core.ApiDoc;

namespace NbSites.ApiDoc.Boots
{
    internal static class ApiDocSetup
    {
        internal static IServiceCollection AddApiDoc(this IServiceCollection services, ApiDocInfoRegistry registry)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;

            services.AddSwaggerGen(setupAction =>
            {
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var apiDocInfos = registry.ApiDocInfos;

                    //load xml comments
                    var xmlFiles = apiDocInfos.Select(x => x.XmlFile).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                    foreach (var xmlFile in xmlFiles)
                    {
                        var xmlFilePath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlFilePath))
                        {
                            setupAction.IncludeXmlComments(xmlFilePath);
                        }
                    }

                    //load groups
                    foreach (var apiDocInfo in apiDocInfos)
                    {
                        setupAction.SwaggerDoc(apiDocInfo.Name, new OpenApiInfo()
                        {
                            Title = apiDocInfo.Title,
                            Version = apiDocInfo.Version,
                            Description = apiDocInfo.Description
                        });
                    }
                }
            });

            return services;
        }

        internal static IApplicationBuilder UseApiDoc(this IApplicationBuilder app, ApiDocInfoRegistry registry)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                ////serve the Swagger UI at the root (http://localhost:<port>/)
                //c.RoutePrefix = string.Empty;

                //switch "~/swagger/index.html" to "~/SwaggerHide/index.html", so now we use the only mvc "~/ApiDoc" entry
                c.RoutePrefix = "SwaggerHide";

                foreach (var apiDocInfo in registry.ApiDocInfos)
                {
                    c.SwaggerEndpoint(apiDocInfo.Endpoint, apiDocInfo.Name);
                }
            });
            return app;
        }
    }
}
