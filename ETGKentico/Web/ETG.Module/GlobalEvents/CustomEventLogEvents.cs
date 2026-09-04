using CMS;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Search;
using CommonServiceLocator;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Repositories;
using ETG.Core.Search;
using ETG.Module.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CMS.EventLog;

[assembly: RegisterModule(typeof(CustomEventLogEvents))]
public class CustomEventLogEvents : Module
{
    // Module class constructor, the system registers the module under the name "CustomInit"
    public CustomEventLogEvents()
        : base("EventLogEvents")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit()
    {
        base.OnInit();

        // Assigns custom handlers to events
        EventLogEvents.LogEvent.Before += EventLog_Insert_Before;

    }

    
    private void EventLog_Insert_Before(object sender, LogEventArgs e)
    {
        EventLogInfo eventLogRecord = e.Event;

        // Disables event log records if its BaseTime has to precede currentTime,
        string eventCode = eventLogRecord.EventDescription;
        if (eventCode.Contains("BaseTime has to precede currentTime"))
        {
            e.Cancel();
        }
    }
}