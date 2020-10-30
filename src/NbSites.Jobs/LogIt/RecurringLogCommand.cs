using System;
using System.Threading.Tasks;
using Hangfire;
using NbSites.Core.AutoInject;
using NbSites.Core.Jobs;

namespace NbSites.Jobs.LogIt
{
    public class RecurringLogCommand : IBackgroundCommand, IAutoInjectAsTransient
    {
        private readonly FooService _fooService;
        private readonly FooDbContext _dbContext;

        public RecurringLogCommand(FooService fooService, FooDbContext dbContext)
        {
            _fooService = fooService;
            _dbContext = dbContext;
        }

        public object Args { get; set; }

        public Task Invoke(object methodArgs)
        {
            var append = " s:" +_fooService.Id + ", ctx: " + _dbContext.Id;
            return LogCommandHelper.Instance.Log(this.GetType(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " with args " + methodArgs + append);
        }

        public string Enqueue()
        {
            this.Recurring(Cron.Minutely);
            return null;
        }
    }

    public class FooDbContext : IAutoInjectAsScoped
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public class FooService : IAutoInjectAsSingleton
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}