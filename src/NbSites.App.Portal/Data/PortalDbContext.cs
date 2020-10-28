using Microsoft.EntityFrameworkCore;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Base.Data;

namespace NbSites.App.Portal.Data
{
    public class PortalDbContext
    {
        public PortalDbContext(NbSitesDbContext dbContext)
        {
            NbSitesDbContext = dbContext;
        }

        public NbSitesDbContext NbSitesDbContext { get;}

        public DbSet<Blog> Blogs => NbSitesDbContext.Set<Blog>();
        public int SaveChanges() => NbSitesDbContext.SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess) => NbSitesDbContext.SaveChanges(acceptAllChangesOnSuccess);
    }
}