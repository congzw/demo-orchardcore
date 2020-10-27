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
}
