using System;

namespace ETG.Core.Services
{
    public interface ILogger
    {
        void LogInformation(string source, string eventCode, string eventDescription);
        void LogException(string source, string eventCode, Exception ex, string additionalMessage);
    }
}
