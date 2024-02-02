using Gurock.SmartInspect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading;


namespace SmartLogger
{
    public enum Level
    {
        Trace,
        Debug,
        Verbose,
        Info,
        Message,
        Warn,
        Warning,
        Error,
        Fatal,
        Control,
        Off,
        Standby
    }

    public sealed class HyperLogger
    {
        private static HyperLogger instance = null;
        private IDictionary<string, HyperLog> sessionLookup;
        private List<HyperLog> sessionList;
        private SmartInspect _SmartInspect;
        private ColorManager _ColorManager;
        private static readonly object padlock = new object();
        private Configuration _Configuration;
        private string _ConfigFile;
        private FileSystemWatcher _ConfigFileWatcher;
        private static bool InStartup;

        HyperLogger()
        {
        }

        public static Configuration HyperConfiguration()
        {
            if (instance is HyperLogger)
            {
              return instance._Configuration;  
            }
            else
            {
                return new Configuration(true);  // production config as default
            }
            
        }

        public static List<HyperLog> FindMaskedAreas(string mask)
        {
            string[] masks = mask.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            bool negate;
            bool wildcard;
            char lastChar;
            List<HyperLog> result = new List<HyperLog>();
            foreach (string submask in masks)
            {
                string workingmask = submask.Trim();
                workingmask = workingmask.ToLower();
                if (workingmask == "")
                {
                    continue;
                }
                negate = workingmask[0] == '!';
                if (negate)
                {
                    workingmask = workingmask.Substring(1);
                }
                if (workingmask == "")
                {
                    continue;
                }
                if (workingmask == "*")
                {
                    foreach (HyperLog hyperlog in instance.sessionList)
                    {
                        if (negate)
                        {
                            result.Remove(hyperlog);
                        }
                        else
                        {
                            result.Add(hyperlog);
                        }

                    }
                    continue;
                }
                lastChar = workingmask[workingmask.Length - 1];
                wildcard = (lastChar == '*') || (lastChar == '.');
                if (wildcard)
                {
                    workingmask = workingmask.Substring(0, workingmask.Length - 1);
                }
                bool found;
                foreach (HyperLog hyperlog in instance.sessionList)
                {
                    found = false;
                    if (wildcard)
                    {
                        found = hyperlog.Name.ToLower().StartsWith(workingmask);
                    }
                    else
                    {
                        found = hyperlog.Name.ToLower() == workingmask;
                    }
                    if (found)
                    {
                        if (negate)
                        {
                            result.Remove(hyperlog);
                        }
                        else
                        {
                            result.Add(hyperlog);
                        }

                    }
                }
                continue;
            }
            return result;
        }

        private static void ApplyRuleAreaItem(RuleAreaItem ruleAreaItem)
        {
            List<HyperLog> logs;
            logs = FindMaskedAreas(ruleAreaItem.Mask);
            foreach (HyperLog hyperlog in logs)
            {
                Bootstrap.Log.LogDebug($">>>>>>>>Filtered Area [{hyperlog.Name}]");
                hyperlog.SetHyperLogLevels(ruleAreaItem.LoggingLevel, ruleAreaItem.DetailLevel);
            }
        }
        public static void ReapplyRules()
        {
            Bootstrap.Log.LogColored(Color.Green, $"Reappying Rules count: {instance._Configuration.Rules.Count}");
            foreach (Rule rule in instance._Configuration.Rules)
            {
                Bootstrap.Log.LogColored(Color.YellowGreen, $"Rule [{rule.Name}] Enabled: {rule.Enabled.ToString()}");
                if (rule.Enabled)
                {
                    foreach (RuleAreaItem item in rule.RuleAreaItems)
                    {
                        if (item.Enabled)
                        {
                            Bootstrap.Log.LogColored(Color.LightYellow, $">>>Rule item[{item.Mask}]");
                            ApplyRuleAreaItem(item);
                        }

                    }

                }
            }
        }

        public static string CurrentStateToJSONstring()
        {
            List<RuntimeHyperArea> areas = new List<RuntimeHyperArea>();
            RuntimeHyperArea area;
            foreach (HyperLog hyperlog in instance.sessionList)
            {
                area = new RuntimeHyperArea();

                area.Name = hyperlog.Name;
                area.HighLevel = hyperlog.HighLevel();
                area.LowLevel = hyperlog.LowLevel();
                if (hyperlog.Log != null)
                {
                    area.Enabled = hyperlog.Log.SiSession.Active.ToString();
                    area.Color = hyperlog.Log.SiSession.Color.Name;
                    area.Level = hyperlog.Log.SiSession.Level.ToString();

                }
                else
                {
                    area.Enabled = "False";
                    area.Color = "Black";
                    area.Level = "Off";
                }
                areas.Add(area);
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // For pretty-printing the JSON
            };

            return JsonSerializer.Serialize(areas, options);

        }
        public static void UpdateStateFromJSONstring(string json)
        {
                // Ensure to specify the type to deserialize to, in this case, List<RuntimeHyperArea>
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // This option helps with deserializing if the JSON property names don't match case with your C# model properties
                };

                List<RuntimeHyperArea> areas = JsonSerializer.Deserialize<List<RuntimeHyperArea>>(json, options);

            if (areas != null) // Always a good practice to check for null after deserialization
            {

                foreach (RuntimeHyperArea area in areas)
                {
                    // Your logic here

                }
            }
        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (InStartup)
            {
                return;
            }
            Bootstrap.Log.LogColored(Color.Black, $"OnChanged Event");
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Bootstrap.Log.LogColored(Color.Brown, $"On Config file Changed Event");
            int retries = 10;
            Configuration updatedConfig = null;
            Thread.Sleep(100);
            while (retries > 0)
            {
                try
                {
                    updatedConfig = Configuration.LoadConfiguration(instance._ConfigFile);
                    break;

                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(100);
                    updatedConfig = null;
                    Bootstrap.Log.LogException($"Issue reading file Retry{retries}", ex);
                }
            }

            if (!(updatedConfig is Configuration))
            {
                Bootstrap.Log.LogWarning($"Issues encountered when reloading the configuration file {instance._ConfigFile}. The current configuration is still being used.");
                return;
            }
            instance._Configuration = updatedConfig;
            HyperLog hyperlog;
            foreach (Area area in instance._Configuration.Areas)
            {
                if (instance.sessionLookup.TryGetValue(area.Name.ToLower(), out hyperlog))
                {
                    hyperlog.UpdateFromArea(area);
                }

            }
            ReapplyRules();
        }

        public static string ConfigFile(string exeFile = "")
        {
            return LogFilePicker.ConfigFile(exeFile);
        }

        public static HyperLog MakeHyperLog(string name)
        {
            string key;
            HyperLog x;
            lock (padlock)
            {
                if (instance == null)
                {
                    InStartup = true;
                    Bootstrap.Log.LogMessage("Starting Up Logger");
                    instance = new HyperLogger();
                    instance._ConfigFile = ConfigFile();
                    instance._Configuration = Configuration.LoadConfiguration(instance._ConfigFile);
                    if (instance._Configuration.Rules.Count == 0) // In case rules manually removed from config file
                    {
                        instance._Configuration.AddDefaultRules(true); // production default
                    }
 
                    Bootstrap.Log.LogColored(Color.MintCream, System.IO.Path.GetDirectoryName(instance._ConfigFile));
                    instance._ConfigFileWatcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(instance._ConfigFile) + "\\");
                    Bootstrap.Log.LogColored(Color.MintCream, System.IO.Path.GetFileName(instance._ConfigFile));

                    instance._ConfigFileWatcher.Filter = System.IO.Path.GetFileName(instance._ConfigFile);
                    instance._ConfigFileWatcher.Changed += OnChanged;
                    instance._ConfigFileWatcher.EnableRaisingEvents = true;
                    instance._SmartInspect = new SmartInspect(Configuration.PlainAppFilename());
                    instance.sessionLookup = new Dictionary<string, HyperLog>();
                    instance.sessionList = new List<HyperLog>();
                    instance._SmartInspect.AppName = instance._Configuration.AppName;
                    instance._SmartInspect.Enabled = instance._Configuration.Enabled;
                    instance._SmartInspect.Resolution = instance._Configuration.Resolution.ToLower() == "high" ? ClockResolution.High : ClockResolution.Standard;
                    instance._SmartInspect.DefaultLevel = HyperAreaBase.XlatLevel(instance._Configuration.DefaultLevel);
                    instance._SmartInspect.SessionDefaults.Level = HyperAreaBase.XlatLevel(instance._Configuration.SessionDefaults);
                    instance._SmartInspect.Connections = instance._Configuration.ConfigurationString();
                    //Bootstrap.Log.LogString(Gurock.SmartInspect.Level.Warning, "Connections", instance._SmartInspect.Connections);
                    instance._SmartInspect.Enabled = true;
                    instance._ColorManager = new ColorManager();
                    Bootstrap.Log.LogString(Gurock.SmartInspect.Level.Debug, "Connections", instance._SmartInspect.Connections);
                    Bootstrap.Log.LogInt(Gurock.SmartInspect.Level.Debug, "Area Count", instance._Configuration.Areas.Count);
                    Bootstrap.Log.LogInt(Gurock.SmartInspect.Level.Debug, "Rule Count", instance._Configuration.Rules.Count);
                    Bootstrap.Log.LogInt(Gurock.SmartInspect.Level.Debug, "Connection Count", instance._Configuration.Connections.Count);
                    foreach (Area area in instance._Configuration.Areas)
                    {
                        key = area.Name.ToLower();
                        Bootstrap.Log.LogString("Area From Config", area.Name);
                        x = new HyperLog(instance._SmartInspect, area);
                        instance.sessionLookup.Add(key, x);
                        instance.sessionList.Add(x);
                        instance._ColorManager.NextColor(); // bump to reduce duplicate colors.
                    }
                    ReapplyRules();
                    // Bootstrap.Log.LogString(Gurock.SmartInspect.Level.Warning, "XNew Connections", instance._Configuration.ConfigurationString());
                    // Log the messages
                    InStartup = false;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    key = name.ToLower();
                    if (!instance.sessionLookup.TryGetValue(key, out x))
                    {
                        Area newArea = new Area();
                        newArea.Name = name;
                        newArea.AreaColor = instance._ColorManager.NextColor().Name;
                        x = new HyperLog(instance._SmartInspect, newArea);
                        instance.sessionLookup.Add(key, x);
                        instance.sessionList.Add(x);
                        x.High.Warning(name, Color.Fuchsia);
                        instance._Configuration.Areas.Add(newArea);
                        ReapplyRules();
                    }
                    //File.WriteAllText(@"f:\json.txt", CurrentStateToJSONstring());
                    return x;

                }
                return null;
            }
        }
        public static HyperLog MakeHyperLog(object unit)
        {
            string name = unit.GetType().ToString();
            return MakeHyperLog(name);
        }
    }
}







