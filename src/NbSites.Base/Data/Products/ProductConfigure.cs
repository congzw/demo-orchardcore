using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NbSites.Base.Data.Products
{
    public class ProductConfigure : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("BaseLib_Products_Product");

            builder.HasMany(x => x.ProductVersions)
                .WithOne()
                .HasForeignKey(x => x.ProductId)
                .HasConstraintName("FK_ProductVersion_Product");
        }
    }

    //public class MyClass : IModuleBuilderApply
    //{
    //    public void Apply(ModelBuilder builder)
    //    {
    //        var product = builder.Entity<Product>();
    //        product.ToTable("Base_Products_Product");
    //        product.HasMany(x => x.ProductVersions)
    //            .WithOne()
    //            .HasForeignKey(x => x.ProductId)
    //            .HasConstraintName("FK_ProductVersion_Product");
            
    //        var productVersion = builder.Entity<ProductVersion>();
    //        productVersion.ToTable("Base_Products_ProductVersion");
    //    }
    //}

}
