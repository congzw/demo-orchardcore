using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace NbSites.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(builder =>
                //{
                //    builder.AddJsonFile("config/tenants.json");
                //    builder.AddJsonFile(".config/appsettings.json", true, true);
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
