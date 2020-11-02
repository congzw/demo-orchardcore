using Microsoft.EntityFrameworkCore;

namespace NbSites.Jobs.Hangfires
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    //public class HangfireDbContext : DbContext
    //{
    //    private readonly string _dbConn;

    //    public HangfireDbContext(string dbConn)
    //    {
    //        _dbConn = dbConn;
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        //optionsBuilder.UseMySQL(_dbConn);
    //        optionsBuilder.UseSqlServer(_dbConn);
    //    }
    //}
}
