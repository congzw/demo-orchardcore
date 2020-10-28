using System.Linq;
using NbSites.Base.Data.Products;
using NbSites.Core.DataSeed;

namespace NbSites.Base.Data
{
    public class BaseSeed : ISeed
    {
        private readonly NbSitesDbContext _dbContext;

        public BaseSeed(NbSitesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Init()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Product() {Name = "DemoApp", Description = "For DEMO"});
                _dbContext.SaveChanges();
            }
        }
    }
}
