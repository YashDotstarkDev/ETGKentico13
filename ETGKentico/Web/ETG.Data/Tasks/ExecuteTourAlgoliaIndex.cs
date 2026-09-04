using System;
using System.Linq;
using Castle.Core.Internal;
using CMS.EventLog;
using CMS.Scheduler;
using CMS.Search;
using CommonServiceLocator;
using ETG.Core.Search;

namespace ETG.Data.Tasks
{
    public class ExecuteTourAlgoliaIndexTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                var searchService = ServiceLocator.Current.GetInstance<IETGSearchService>();
            
                var message = searchService.RebuildTourSearchIndex();

                if (message.IsNullOrEmpty())
                {
                    return "Last task created at " + DateTime.Now.ToString("G");
                }
      
                return "Could not find relevant search index. Re-building index skipped";
      
            }
            catch (Exception exception)
            {
                return $"Failed to run the task. The exception is {exception.Message}";
            }
        }
    }
}