using NbSites.Core.AutoTasks;

namespace NbSites.Base.Data
{
    public class EnsureDbExist : IAfterAllModulesLoadTask
    {
        private readonly BaseDbContext _dbContext;

        public EnsureDbExist(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Run()
        {
            _dbContext.Database.EnsureCreated();
        }

        public string Category { get; set; } = "EnsureDbExist";
        public int Order { get; set; }
    }
}