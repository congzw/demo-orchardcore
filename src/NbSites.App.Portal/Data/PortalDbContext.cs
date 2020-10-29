using Microsoft.EntityFrameworkCore;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Base.Data;

namespace NbSites.App.Portal.Data
{
    public class PortalDbContext
    {
        public PortalDbContext(BaseDbContext dbContext)
        {
            BaseDbContext = dbContext;
        }

        public BaseDbContext BaseDbContext { get;}

        public DbSet<Blog> Blogs => BaseDbContext.Set<Blog>();
        public int SaveChanges() => BaseDbContext.SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess) => BaseDbContext.SaveChanges(acceptAllChangesOnSuccess);
    }
}