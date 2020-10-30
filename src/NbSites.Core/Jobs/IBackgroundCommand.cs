using System.Threading.Tasks;

namespace NbSites.Core.Jobs
{
    /// <summary>
    /// 后台任务的方法调用
    /// </summary>
    public interface IBackgroundCommandMethod
    {
        /// <summary>
        /// 后台任务的方法参数
        /// </summary>
        object Args { get; set; }
        /// <summary>
        /// 后台任务的方法逻辑，此方法传入的methodArgs，将被Hangfire序列化，并在触发时使用
        /// </summary>
        /// <param name="methodArgs"></param>
        /// <returns></returns>
        Task Invoke(object methodArgs);
    }
    
    /// <summary>
    /// 后台任务的入队逻辑，例如一次，延迟，重复等场景
    /// </summary>
    public interface IBackgroundCommand : IBackgroundCommandMethod
    {
        /// <summary>
        /// 后台任务的入队逻辑，例如一次，延迟，重复等场景，实现者利用Hangfire提供的Job类型按需实现
        /// </summary>
        /// <returns></returns>
        string Enqueue();
    }
}