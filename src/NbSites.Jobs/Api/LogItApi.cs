using Microsoft.AspNetCore.Mvc;
using NbSites.Jobs.LogIt;

namespace NbSites.Jobs.Api
{
    [ApiExplorerSettings(GroupName = "Demo-Jobs")]
    [Route("~/Api/Jobs/LogIt/[action]")]
    [ApiController]
    public class LogItApi : ControllerBase
    {
        [HttpGet]
        public string GetInfo()
        {
            return this.GetType().FullName;
        }

        [HttpGet]
        public AsyncLogCommand AsyncLog([FromServices] AsyncLogCommand cmd)
        {
            cmd.Args = "Hello Async Call";
            cmd.Enqueue();
            return cmd;
        }

        [HttpGet]
        public SyncLogCommand SyncLog([FromServices] SyncLogCommand cmd)
        {
            cmd.Args = "Hello Sync Call";
            cmd.Enqueue();
            return cmd;
        }

        [HttpGet]
        public DelayCallCommand DelayCall()
        {
            var cmd = new DelayCallCommand();
            cmd.Args = "Hello Delay Async Call";
            cmd.Enqueue();
            return cmd;
        }

        [HttpGet]
        public RecurringLogCommand RecurringLog([FromServices] RecurringLogCommand cmd)
        {
            cmd.Args = "Hello Recurring Call";
            cmd.Enqueue();
            return cmd;
        }
    }
}
