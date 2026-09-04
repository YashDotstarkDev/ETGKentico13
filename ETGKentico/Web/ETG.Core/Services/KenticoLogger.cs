using System;
using CMS.EventLog;
using ETG.Core.Kentico;

namespace ETG.Core.Services
{
    public class KenticoLogger : ILogger
    {
        private readonly ISiteContext _siteContext;
        public KenticoLogger(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public void LogInformation(string source, string eventCode, string eventDescription)
        {
            EventLogProvider.LogInformation(source, eventCode, eventDescription);
        }

        public void LogException(string source, string eventCode, Exception ex, string additionalMessage)
        {
            EventLogProvider.LogException(source, eventCode, ex, _siteContext.SiteId,additionalMessage);
        }
    }
}
