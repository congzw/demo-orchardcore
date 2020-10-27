//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace NbSites.Base.Data
//{
//    public class NbSitesDbContextDesignTimeFactory : IDesignTimeDbContextFactory<NbSitesDbContext>
//    {
//        public NbSitesDbContext CreateDbContext(string[] args)
//        {
//            var dbContextBuilder = new DbContextOptionsBuilder<NbSitesDbContext>();
//            var connString = "Server=(localdb)\\MSSQLLocalDB; Database=NbSitesDb-v1; Trusted_Connection=True; MultipleActiveResultSets=true";
//            dbContextBuilder.UseSqlServer(connString);
//            return new NbSitesDbContext(dbContextBuilder.Options);
//        }
//    }
//}
