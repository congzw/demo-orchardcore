using System.Linq;
using NbSites.App.Portal.Data.Blogs;
using NbSites.Core.AutoInject;
using NbSites.Core.AutoTasks;

namespace NbSites.App.Portal.Data
{
    public class PortalSeed : IAfterAllModulesLoadTask, IAutoInjectAsScoped
    {
        private readonly PortalDbContext _dbContext;

        public PortalSeed(PortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string Category { get; } = "DataSeed";
        public int Order { get; set; }

        public void Run()
        {
            var blogs = _dbContext.Blogs;
            if (!blogs.Any())
            {
                blogs.Add(new Blog() { Name = "Blog A" });
                blogs.Add(new Blog() { Name = "Blog B" });
                _dbContext.SaveChanges();
            }
        }
    }
}
