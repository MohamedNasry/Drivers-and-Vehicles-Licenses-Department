using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Loggin
{
    public class EventLogger
    {
        private const string source = "DVLDApplication";
        private const string logName = "Application";

        public static void LogError(Exception ex, string context)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, logName);
                }

                string message = $"{context}: {ex.Message}\n{ex.StackTrace}";

                EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
            catch
            {
            }
        }

    
    }
}
