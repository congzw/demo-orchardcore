using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Base.Data;
using NbSites.Base.Data.Products;
using NbSites.Core.EFCore;

namespace NbSites.Migrations
{
    public class LightDbDesign : IDesignTimeDbContextFactory<BaseDbContext>
    {
        public BaseDbContext CreateDbContext(string[] args)
        {
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(typeof(ProductConfigure).Assembly);
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(typeof(Blog).Assembly);

            var dbContextBuilder = new DbContextOptionsBuilder<BaseDbContext>();
            var connString = "Server=(localdb)\\MSSQLLocalDB; Database=LightDb-v1; Trusted_Connection=True; MultipleActiveResultSets=true";
            dbContextBuilder.UseSqlServer(connString, b => b.MigrationsAssembly("NbSites.Migrations"));
            var nbSitesDbContext = new BaseDbContext(dbContextBuilder.Options);
            return nbSitesDbContext;
        }
    }
}
