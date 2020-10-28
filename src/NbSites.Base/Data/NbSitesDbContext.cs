using Microsoft.EntityFrameworkCore;
using NbSites.Base.Data.Products;
using NbSites.Core.EFCore;

namespace NbSites.Base.Data
{
    public class NbSitesDbContext : DbContext
    {
        public NbSitesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var modelConfigAssemblies = ModelAssemblyRegistry.Instance.Assemblies;
            foreach (var modelConfigAssembly in modelConfigAssemblies)
            {
                builder.ApplyConfigurationsFromAssembly(modelConfigAssembly);
            }
        }
    }
}
