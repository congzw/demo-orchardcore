using System;
using Hangfire;

namespace NbSites.Core.Jobs
{
    /// <summary>
    /// some useful extension for hangfire
    /// </summary>
    public static class HangfireCommandExtensions
    {
        public static string Once(this IBackgroundCommand cmd)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            return BackgroundJob.Enqueue(() => cmd.Invoke(cmd.Args));
        }

        public static string Schedule(this IBackgroundCommand cmd, TimeSpan delay)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            return BackgroundJob.Schedule(() => cmd.Invoke(cmd.Args), delay);
        }
        
        //# Example of job definition:
        //# .----------------- minute (0 - 59)
        //# |   .------------- hour (0 - 23)
        //# |   |  .---------- day of month (1 - 31)
        //# |   |  |  .------- month (1 - 12) OR jan,feb,mar,apr ...
        //# |   |  |  |  .---- day of week (0 - 6) (Sunday=0 or 7)
        //# |   |  |  |  |
        //# *   *  *  *  *   command to be executed
        //m/n**** command1

        //0 0/10 * * * ?  => every 10 minutes
        
        /// <summary>
        /// Cron.Minutely
        /// Cron.Daily
        /// ...
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cron"></param>
        public static void Recurring(this IBackgroundCommand cmd, Func<string> cron)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            RecurringJob.AddOrUpdate(() => cmd.Invoke(cmd.Args), cron);
        }
    }
}