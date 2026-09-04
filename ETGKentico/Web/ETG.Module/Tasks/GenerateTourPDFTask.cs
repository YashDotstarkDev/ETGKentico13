using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Scheduler;
using CMS.SiteProvider;

namespace ETG.Module.Tasks
{
    public class GenerateTourPDFTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {

                var webClient = new WebClient();
                var tourCode = task.TaskData;

                if (string.IsNullOrEmpty(tourCode))
                {
                    return "Invalid tour code";
                }

                var url = $"{SiteContext.CurrentSite.SitePresentationURL}/pdf/{tourCode}?force=1";
                webClient.DownloadData(url);
                EventLogProvider.LogInformation("PDFCREATE", tourCode, url);
                task.Delete();
                return $"Last run at {DateTime.Now}. ";
            }
            catch (Exception exception)
            {
                return $"Failed to run the task. The exception is {exception.Message}";
            }
        }
    }
}