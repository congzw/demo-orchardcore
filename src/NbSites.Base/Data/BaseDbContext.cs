using Microsoft.EntityFrameworkCore;
using NbSites.Base.Data.Products;
using NbSites.Core.Data;
using NbSites.Core.EFCore;

namespace NbSites.Base.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var modelConfigAssemblies = ModelAssemblyRegistry.Instance.Assemblies;
            foreach (var modelConfigAssembly in modelConfigAssemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(modelConfigAssembly);
            }
            modelBuilder.ApplyAutoLength();
        }
    }
}
