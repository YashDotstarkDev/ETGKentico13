using System;
using System.Diagnostics;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SalesForce.WebServiceClient;
using CMS.Scheduler;
using CMS.SiteProvider;
using ETG.Data.Settings;
using Newtonsoft.Json;

namespace ETG.Data.Tasks
{
    public class CustomEmailQueueCleaner : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                var numberOfRecords =
                    SettingsKeyInfoProvider.GetIntValue(
                        "CMSEmailDeleteBatchSize",
                        "CMSEmailDeleteBatchSize",
                        0,
                        SiteContext.CurrentSiteName);
                
                var archiveDays = SettingsKeyInfoProvider.GetIntValue(
                    "CMSArchiveEmails",
                    "CMSArchiveEmails",
                    0,
                    SiteContext.CurrentSiteName);

                var archiveDate = DateTime.Now.AddDays(-archiveDays);

                EmailInfoProvider.DeleteArchived(
                    SiteContext.CurrentSiteID,
                    archiveDate, 
                    numberOfRecords);

                return $"Removed {numberOfRecords} archived emails prior to {archiveDate.ToString("f")}.";
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException(nameof(CustomEmailQueueCleaner), "EXCEPTION", ex);
                return $"An error has occurred, please check the event log for more details.";
            }
        }
    }
}