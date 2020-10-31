using System;
using System.Threading.Tasks;
using NbSites.Core.AutoInject;
using NbSites.Core.Context;
using NbSites.Core.Jobs;

namespace NbSites.Jobs.LogIt
{
    public class DelayCallCommand : IBackgroundCommand, IAutoInjectAsTransient
    {
        private readonly TenantContext _tenantContext;

        public DelayCallCommand(TenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public object Args { get; set; }
        public Task Invoke(object methodArgs)
        {
            var processedBy = _tenantContext.Tenant;
            return LogCommandHelper.Instance.Log(this.GetType(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " with args " + methodArgs + " ProcessedBy " + processedBy);
        }

        public string Enqueue()
        {
            //return BackgroundJob.Schedule(() => Invoke(Args), TimeSpan.FromSeconds(3));
            return this.Schedule(TimeSpan.FromSeconds(3));
        }
    }
}
