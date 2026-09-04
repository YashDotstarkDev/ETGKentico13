using System;
using System.Linq;
using System.Net;
using CMS;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Scheduler;
using CMS.SiteProvider;
using CMS.Synchronization;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Module.Classes.Info;
using ETG.Module.GlobalEvents;
using ETG.Module.Helpers;
[assembly: RegisterModule(typeof(CustomStagingEvents))]
namespace ETG.Module.GlobalEvents
{
    public class CustomStagingEvents : CMS.DataEngine.Module
    {
        public CustomStagingEvents()
            : base("CustomStagingEvents")
        {
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            StagingEvents.ProcessTask.After += Staging_ProcessTask_After;

        }

        private void CreateGeneratePDFTask(string tourCode)
        {

            TaskInfo task = new TaskInfo
            {
                TaskDisplayName = $"Generate PDF {tourCode}",
                TaskName = $"ETGGeneratePDFTask{tourCode}",
                TaskAssemblyName = "ETG.Module",
                TaskClass = "ETG.Module.Tasks.GenerateTourPDFTask",
                TaskSiteID = SiteContext.CurrentSiteID,
                TaskData = tourCode,
                TaskEnabled = true,
                TaskInterval = $"once;{DateTime.Now.AddMinutes(5):MM/dd/yyyy h:mm:ss tt}",
                TaskRunInSeparateThread = true,
                TaskNextRunTime = DateTime.Now.AddMinutes(5)
            };
            // Sets the basic task properties

            task.Insert();
        }

        private void Staging_ProcessTask_After(object sender, StagingSynchronizationEventArgs e)
        {
            if (e.TaskType == TaskTypeEnum.CreateDocument || e.TaskType == TaskTypeEnum.UpdateDocument)
            {
                try
                {

                    var className = e.TaskData.Tables[0].Rows[0]["ClassName"].ToString();
                    if (className.Equals(Tour.CLASS_NAME) ||
                        className.Equals(TourHighlilght.CLASS_NAME) || className.Equals(TourItinerary.CLASS_NAME))
                    {
                        string tourCode = string.Empty;
                        if (className.Equals(TourHighlilght.CLASS_NAME) || className.Equals(TourItinerary.CLASS_NAME))
                        {
                            var tourPath =
                                PathHelper.Get2LevelUpAliasPath(
                                    e.TaskData.Tables[0].Rows[0]["NodeAliasPath"].ToString());

                            TourProvider.GetTours().Path(tourPath).TopN(1).Select(a => a.TourCode).FirstOrDefault();
                        }
                        else
                        {
                            tourCode = e.TaskData.Tables[0].Rows[0]["TourCode"].ToString();

                        }

                        if (!TaskExistForTourCode(tourCode))
                        {
                            CreateGeneratePDFTask(tourCode);

                        }
                        


                    }
                }
                catch(Exception ex)
                {
                    EventLogProvider.LogException("TOURPDF", "GENERATE", ex);
                }
            }
        }

        private bool TaskExistForTourCode(string tourCode)
        {
            return TaskInfoProvider.GetTasks().WhereEquals("TaskName", $"ETGGeneratePDFTask{tourCode}").Any();

        }
    }
}