using System;
using Hangfire;
using NbSites.Core.AutoInject;

namespace NbSites.Jobs.LogIt
{
    public class SyncLogCommand : IAutoInjectAsTransient
    {
        public object Args { get; set; }

        public void Invoke(object methodArgs)
        {
            LogCommandHelper.Instance.Log(this.GetType(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " with args " + methodArgs);
        }

        public string Enqueue()
        {
            return BackgroundJob.Enqueue(() => Invoke(this.Args));
        }
    }
}