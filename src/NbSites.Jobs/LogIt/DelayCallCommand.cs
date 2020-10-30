using System;
using System.Threading.Tasks;
using NbSites.Core.AutoInject;
using NbSites.Core.Jobs;

namespace NbSites.Jobs.LogIt
{
    public class DelayCallCommand : IBackgroundCommand, IAutoInjectAsTransient
    {
        public object Args { get; set; }
        public Task Invoke(object methodArgs)
        {
            return LogCommandHelper.Instance.Log(this.GetType(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " with args " + methodArgs);
        }

        public string Enqueue()
        {
            //return BackgroundJob.Schedule(() => Invoke(Args), TimeSpan.FromSeconds(3));
            return this.Schedule(TimeSpan.FromSeconds(3));
        }
    }
}
