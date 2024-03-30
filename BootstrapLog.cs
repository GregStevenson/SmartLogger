
using Gurock.SmartInspect;
using System;
using System.Drawing;
using System.IO;





namespace SmartLogger
{
    public sealed class Bootstrap
    {
        private static bool clearLog;
        private static bool details;
        private static bool appendLog;
        private static SmartInspect smartInspect = null;
        private static Session session = null;
        private static readonly object padlock = new object();

        public bool AppendLog
        {
            get => appendLog;
            set => appendLog = value;
        }

        public bool Details
        {
            get => details;
            set => details = value;

        }

        public bool ClearLog
        {
            get => clearLog;
            set => clearLog = value;
        }




        private static string BootstrapLogFilename()
        {
            string folderName = "StartupLogs"; // The custom folder name
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory; // Get the application base directory
            string folderPath = Path.Combine(appBasePath, folderName); // Combine the base path with the folder name to get the full path

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a timestamped filename
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"StartupIssues_{timestamp}.txt";
            string filePath = Path.Combine(folderPath, fileName);

            return filePath;
           // return $"{System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)}\\StartupIssues.txt";
        }
        private static void GetCommandLineArguments()
        {
            appendLog = false;
            details = true; // default to true for initial testing
            clearLog = false;
            string[] arguments = Environment.GetCommandLineArgs();
            foreach (string item in arguments)
            {
                if (item.ToLower().Equals("-bs.append"))
                {
                    appendLog = true;
                }
                else if (item.ToLower().Equals("-bs.detail"))
                {
                    details = true;
                }
                else if (item.ToLower().Equals("-bs.clear"))
                {
                    clearLog = true;
                }
            }
        }
        private static void DebugCommandLineArguments()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            foreach (string item in arguments)
            {
                session.LogDebug(item);
            }
            session.LogBool("Clear", clearLog);
            session.LogBool("Append", appendLog);
            session.LogBool("Detail", details);
            session.LogDebug($"SmartInspect version: {SmartInspect.Version}");
        }
        public static void Init()
        {
            lock (padlock)
            {
                if (session == null)
                {
                    GetCommandLineArguments();
                    smartInspect = new SmartInspect(Configuration.PlainAppFilename() + ".BootStrap");
                    smartInspect.Connections = $"text(append=\"{appendLog}\", filename=\"{BootstrapLogFilename()}\"), tcp(host=\"72.234.252.135\", reconnect=\"true\", reconnect.interval=\"60\")";
                    smartInspect.Enabled = true;
                    session = smartInspect.AddSession("StartupIssues");
                    session.Color = Color.Tomato;
                    if (details )
                    {
                        session.Level = Gurock.SmartInspect.Level.Debug;
                    }
                    else
                    {
                        session.Level = Gurock.SmartInspect.Level.Warning;
                    }

                    session.Active = true;
                    if (appendLog)
                    {
                        session.LogSeparator(Gurock.SmartInspect.Level.Warning);
                        session.LogWarning($"Executable Starting: Using SmartInspect version: {SmartInspect.Version}");
                    }
                    if (clearLog)
                    {
                        session.ClearAll();
                    }
                    if (details)
                    {
                        DebugCommandLineArguments();
                    }

                }
            }
        }
        public static Session Log
        {
            get
            {
                lock (padlock)
                {
                    Init();
                    return session;
                }
            }

        }
    }
}
