using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NbSites.Base.Data.Products
{
    public class ProductConfigure : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToBaseTable("Products_Product");

            builder.HasMany(x => x.ProductVersions)
                .WithOne()
                .HasForeignKey(x => x.ProductId)
                .HasConstraintName("FK_ProductVersion_Product");
        }
    }

    public class ProductVersionConfigure : IEntityTypeConfiguration<ProductVersion>
    {
        public void Configure(EntityTypeBuilder<ProductVersion> builder)
        {
            builder.ToBaseTable("Products_ProductVersion");
        }
    }
}
