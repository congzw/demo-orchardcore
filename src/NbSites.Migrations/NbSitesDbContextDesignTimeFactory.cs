using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Base.Data;
using NbSites.Base.Data.Products;
using NbSites.Core.EFCore;

namespace NbSites.Migrations
{
    public class NbSitesDbContextDesignTimeFactory : IDesignTimeDbContextFactory<NbSitesDbContext>
    {
        public NbSitesDbContext CreateDbContext(string[] args)
        {
            //ModelAssemblyRegistry.Instance.AddModelConfigAssembly("NbSitesDbContext", typeof(ProductConfigure).Assembly);
            //ModelAssemblyRegistry.Instance.AddModelConfigAssembly("NbSitesDbContext", typeof(Blog).Assembly);

            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(typeof(ProductConfigure).Assembly);
            ModelAssemblyRegistry.Instance.AddModelConfigAssembly(typeof(Blog).Assembly);

            var dbContextBuilder = new DbContextOptionsBuilder<NbSitesDbContext>();
            var connString = "Server=(localdb)\\MSSQLLocalDB; Database=NbSitesDb-v1; Trusted_Connection=True; MultipleActiveResultSets=true";
            dbContextBuilder.UseSqlServer(connString, b => b.MigrationsAssembly("NbSites.Migrations"));
            var nbSitesDbContext = new NbSitesDbContext(dbContextBuilder.Options);
            return nbSitesDbContext;
        }
    }
}
