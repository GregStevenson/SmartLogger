using System.IO;

namespace SmartLogger
{
    public enum ConnectionTypes { File, Text, Pipe, Tcp, Mem }
    public enum LogRotate { Disabled, Hourly, Daily, Weekly, Monthly }
    public class Connection
    {
        bool enabled = false;
        bool indentTitle = false;
        string patternString = "[%timestamp%] %level%: %title%";
        LogRotate rotate = LogRotate.Disabled;
        bool appendInsteadOfOverride = false;
        int connectionTimeout = 30000;
        int port = 4228;
        string hostName = "127.0.0.1";
        string namedPipe = "smartinspect";
        bool createTextInsteadOfBinary = false;
        int memoryQueueSize = 2048;
        bool clearQueueOnDisconnect = false;
        bool throttleApplciation = true;
        int asyncQueueSize = 2048;
        bool asynchronousEnabled = false;
        bool alwaysKeepConnectionOpen = false;
        Level flushOnLevel = Level.Error;
        int backlogQueueSize = 2048;
        bool backlogEnabled = false;
        int reconnectInterval = 0;
        bool automaticallyReconnect = false;
        string logCaption = "file";
        Level logLevel = Level.Debug;
        string encryptKey = "";
        bool encryptLogs = false;
        int maximumLogCount = 0;
        int maximumLogSize = 0;
        int fileBufferSize = 0;
        string fileName = "log.sil";
        string filePath = "";
        string connectionName = string.Empty;
        ConnectionTypes connectionType;

        public string ConnectionName
        {
            get 
                {
                 if (string.IsNullOrEmpty(connectionName))
                {
                    return $"{connectionType}-{fileName}";
                }
                else
                {
                    return connectionName;
                }
            }
            set => connectionName = value;
        }

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public ConnectionTypes ConnectionType
        {
            get => connectionType;
            set => connectionType = value;
        }


        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }

        public int FileBufferSize
        {
            get => fileBufferSize;
            set => fileBufferSize = value;
        }


        public bool AppendInsteadOfOverride
        {
            get => appendInsteadOfOverride;
            set => appendInsteadOfOverride = value;
        }

        public int MaximumLogSize
        {
            get => maximumLogSize;
            set => maximumLogSize = value;
        }

        public int MaximumLogCount
        {
            get => maximumLogCount;
            set => maximumLogCount = value;
        }


        public LogRotate Rotate
        {
            get => rotate;
            set => rotate = value;
        }


        public bool EncryptLogs
        {
            get => encryptLogs;
            set => encryptLogs = value;
        }


        public string EncryptKey
        {
            get => encryptKey;
            set => encryptKey = value;
        }

        public Level LogLevel
        {
            get => logLevel;
            set => logLevel = value;
        }

        public string LogCaption
        {
            get => logCaption;
            set => logCaption = value;
        }

        public bool AutomaticallyReconnect
        {
            get => automaticallyReconnect;
            set => automaticallyReconnect = value;
        }

        public int ReconnectInterval
        {
            get => reconnectInterval;
            set => reconnectInterval = value;
        }

        public bool BacklogEnabled
        {
            get => backlogEnabled;
            set => backlogEnabled = value;
        }

        public int BacklogQueueSize
        {
            get => backlogQueueSize;
            set => backlogQueueSize = value;
        }

        public Level FlushOnLevel
        {
            get => flushOnLevel;
            set => flushOnLevel = value;
        }

        public bool AlwaysKeepConnectionOpen
        {
            get => alwaysKeepConnectionOpen;
            set => alwaysKeepConnectionOpen = value;
        }

        public bool AsynchronousEnabled
        {
            get => asynchronousEnabled;
            set => asynchronousEnabled = value;
        }

        public int AsyncQueueSize
        {
            get => asyncQueueSize;
            set => asyncQueueSize = value;
        }

        public bool ThrottleApplciation
        {
            get => throttleApplciation;
            set => throttleApplciation = value;
        }

        public bool ClearQueueOnDisconnect
        {
            get => clearQueueOnDisconnect;
            set => clearQueueOnDisconnect = value;
        }

        public int MemoryQueueSize
        {
            get => memoryQueueSize;
            set => memoryQueueSize = value;
        }

        public bool CreateTextInsteadOfBinary
        {
            get => createTextInsteadOfBinary;
            set => createTextInsteadOfBinary = value;
        }

        public string NamedPipe
        {
            get => namedPipe;
            set => namedPipe = value;
        }

        public string HostName
        {
            get => hostName;
            set => hostName = value;
        }

        public int Port
        {
            get => port;
            set => port = value;
        }

        public int ConnectionTimeout
        {
            get => connectionTimeout;
            set => connectionTimeout = value;
        }


        public string PatternString
        {
            get => patternString;
            set => patternString = value;
        }



        public bool IndentTitle
        {
            get => indentTitle;
            set => indentTitle = value;
        }


        string RotateFragment()
        {
            string fragment = "";
            if (MaximumLogCount > 0)
            {
                fragment += $", maxparts=\"{MaximumLogCount}\"";
            }
            if (MaximumLogSize > 0)
            {
                fragment += $", maxsize=\"{MaximumLogSize}\"";
            }
            if (Rotate != LogRotate.Disabled)
            {
                fragment += $", rotate=\"{rotate.ToString().ToLower()}\"";
            }
            return fragment;
        }
        string EncryptionFragment()
        {

            string fragment = "";
            if (EncryptLogs)
            {
                fragment += $", encrypt=\"true\"";
            }
            if (EncryptKey != "")
            {
                fragment += $", key=\"{EncryptKey}\"";
            }
            return fragment;
        }

        string GeneralFragment()
        {

            string fragment = "";
            if (LogCaption != ConnectionType.ToString().ToLower())
            {
                fragment += $", caption=\"{LogCaption}\"";
            }
            if (AutomaticallyReconnect)
            {
                fragment += $", reconnect=\"true\"";
            }
            if (ReconnectInterval > 0)
            {
                fragment += $", reconnect.interval=\"{ReconnectInterval}\"";
            }
            if ((LogLevel != Level.Debug) || true)
            {
                fragment += $", level=\"{LogLevel.ToString().ToLower()}\"";
            }
            return fragment;
        }
        string BacklogFragment()
        {
            string fragment = "";
            if (BacklogEnabled)
            {
                fragment += $", backlog.enabled=\"true\"";
            }
            if (AlwaysKeepConnectionOpen)
            {
                fragment += $", backlog.keepopen=\"true\"";
            }
            if (BacklogQueueSize != 2048) // default is 2MB
            {
                fragment += $", backlog.queue=\"{BacklogQueueSize}\"";
            }
            if (FlushOnLevel != Level.Error)
            {
                fragment += $", backlog.flushon=\"{FlushOnLevel.ToString().ToLower()}\"";
            }
            return fragment;

        }
        string AsyncFragment()
        {
            string fragment = "";
            if (AsynchronousEnabled)
            {
                fragment += $", async.enabled=\"true\"";
            }
            if (!ThrottleApplciation)
            {
                fragment += $", async.throttle=\"false\"";
            }
            if (ClearQueueOnDisconnect)
            {
                fragment += $", async.clearondisconnect=\"true\"";
            }
            if (AsyncQueueSize != 2048) // default is 2MB
            {
                fragment += $", async.queue=\"{AsyncQueueSize}\"";
            }
            return fragment;
        }
        string FileFragment(string defaultName)
        {
            filePath = filePath.Replace('/', Path.DirectorySeparatorChar);
            if (!FilePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                FilePath += Path.DirectorySeparatorChar;
            }
            string fragment = "";
            if ((FileName != defaultName) || (FilePath != ""))
            {
                fragment += $", filename=\"{FilePath}{FileName}\"";
            }
            if (FileBufferSize > 0)
            {
                fragment += $", buffer=\"{FileBufferSize}\"";
            }
            if (AppendInsteadOfOverride)
            {
                fragment += $", append=\"true\"";
            }
            return fragment;
        }

        string MemoryFragment()
        {
            string fragment = "";
            if (MemoryQueueSize != 2048) // default is 2MB
            {
                fragment += $", maxsize=\"{MemoryQueueSize}\"";
            }
            if (CreateTextInsteadOfBinary)
            {
                fragment += $", astext=\"true\"";
            }
            return fragment;
        }
        string PatternFragment()
        {
            string fragment = "";
            if (PatternString != "[%timestamp%] %level%: %title%") // Default pattern
            {
                fragment += $", pattern=\"{PatternString.Replace("\"", "\"\"")}\"";
            }
            if (IndentTitle)
            {
                fragment += $", indent=\"true\"";
            }
            return fragment;
        }

        string PipeFragment()
        {
            string fragment = "";
            if (NamedPipe != "smartinspect") // Default pattern
            {
                fragment += $", pipename\"{NamedPipe}\"";
            }
            return fragment;
        }

        string TcpFragment()
        {
            string fragment = "";
            if (HostName != "127.0.0.1") // loopback is default
            {
                fragment += $", host=\"{HostName}\"";
            }
            if (Port != 4228) // 4228 is default port
            {
                fragment += $", port=\"{Port}\"";
            }
            if (ConnectionTimeout != 30000) // 30 seconds is default timeout
            {
                fragment += $", timeout=\"{ConnectionTimeout}\"";
            }
            return fragment;
        }

        public string ConnectionString()
        {
            string fragment = ConnectionType.ToString().ToLower() + "(";
            switch (ConnectionType)
            {
                case ConnectionTypes.File:
                    fragment += FileFragment("log.sil");
                    fragment += RotateFragment();
                    fragment += EncryptionFragment();
                    break;
                case ConnectionTypes.Mem:
                    fragment += MemoryFragment();
                    if (CreateTextInsteadOfBinary)
                    {
                        fragment += PatternFragment();
                    }
                    fragment += GeneralFragment();
                    fragment += BacklogFragment();
                    fragment += AsyncFragment();
                    break;
                case ConnectionTypes.Pipe:
                    fragment += PipeFragment();
                    break;
                case ConnectionTypes.Tcp:
                    fragment += TcpFragment();
                    break;
                case ConnectionTypes.Text:
                    fragment += FileFragment("log.txt");
                    fragment += RotateFragment();
                    fragment += PatternFragment();
                    break;
            }
            fragment += GeneralFragment();
            fragment += BacklogFragment();
            fragment += AsyncFragment();
            fragment += ")";
            return fragment.Replace("(, ", "(");
        }
























    }
}
