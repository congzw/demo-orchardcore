using Microsoft.Extensions.Configuration;

namespace Common.Data
{
    public static partial class DbContextHelperExtensions
    {
        public static string GetDataProviderFromConfiguration(this DbContextHelper helper, IConfiguration configuration)
        {
            //template for non tenant
            var dataProvider = configuration["DataProvider"];
            var fixProvider = helper.AutoFixProvider(dataProvider);
            return fixProvider;
        }
    }
}