using System.Linq;
using NbSites.Base.Data.Products;
using NbSites.Core.AutoTasks;

namespace NbSites.Base.Data
{
    public class BaseSeed : IAfterAllModulesLoadTask
    {
        private readonly NbSitesDbContext _dbContext;

        public BaseSeed(NbSitesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Category { get; set; } = "DataSeed";
        public int Order { get; set; }

        public void Run()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Product() {Name = "DemoApp", Description = "For DEMO"});
                _dbContext.SaveChanges();
            }
        }

    }
}
