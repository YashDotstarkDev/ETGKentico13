using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Helpers;
using CMS.Scheduler;

namespace ETG.Module.Tasks
{
    public class ClearCacheTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                CacheHelper.ClearCache();
                return $"Last run at {DateTime.Now}. ";
            }
            catch (Exception exception)
            {
                return $"Failed to run the task. The exception is {exception.Message}";
            }
        }
    }
}