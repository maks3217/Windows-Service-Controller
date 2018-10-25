using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BibliotekaKontoler
{
    public class Loger
    {
        const string LogName = "Kontroler";
        const string LogSource = "KontrolerAPP";



        public Loger()
        {
        }
        public static void Write(string message, string typeName = "information")
        {
            EventLogEntryType type = EventLogEntryType.Information;

            if (typeName == "warning")
            {
                type = EventLogEntryType.Warning;
            }

            if (typeName == "error")
            {
                type = EventLogEntryType.Error;
            }

            if (!isLogCreated())
            {
                CreateLog();
                return;
            }

            EventLog DemoLog = new EventLog(LogName);
            DemoLog.Source = LogSource;
            DemoLog.WriteEntry(message, type);

        }

        public static void CreateLog()
        {
            EventLog.CreateEventSource(LogSource, LogName);
        }

        public static Boolean isLogCreated()
        {
            return EventLog.SourceExists(LogSource, ".");
        }

        public static void LogDelete()
        {
            EventLog.Delete(LogName);
        }

        public static void LogClear()
        {
            EventLog DemoLog = new EventLog(LogName);
            DemoLog.Clear();
        }
    }
}
