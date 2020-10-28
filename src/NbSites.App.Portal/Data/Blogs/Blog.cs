using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NbSites.App.Portal.Data.Blogs
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BlogConfigure : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("App_Portal_Blog");
        }
    }
}
